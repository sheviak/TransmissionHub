using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using TransmissionHub.Client.Internal.Http;

namespace TransmissionHub.Client.Tests.Internal.Http;

public class SessionIdDelegatingHandlerTests
{
    private const string SessionIdHeader = "X-Transmission-Session-Id";

    private static (SessionIdDelegatingHandler handler, MockHttpMessageHandler mock) CreatePipeline()
    {
        var mock = new MockHttpMessageHandler();
        var handler = new SessionIdDelegatingHandler(NullLogger<SessionIdDelegatingHandler>.Instance) { InnerHandler = mock };
        return (handler, mock);
    }

    [Test]
    public async Task SendAsync_NoStoredSessionId_DoesNotAddHeader()
    {
        // Arrange
        var (handler, mock) = CreatePipeline();
        mock.SetResponse(HttpStatusCode.OK);

        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/rpc");

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        await Assert.That(mock.LastRequest!.Headers.Contains(SessionIdHeader)).IsFalse();
    }

    [Test]
    public async Task SendAsync_StoredSessionId_AttachesHeader()
    {
        // Arrange
        var (handler, mock) = CreatePipeline();
        // Prime the handler with a session ID by sending a 409 first
        mock.SetResponse(HttpStatusCode.Conflict, sessionId: "initial-session");
        using var client = new HttpClient(handler);

        using var primeRequest = new HttpRequestMessage(HttpMethod.Post, "http://localhost/rpc");
        using var _ = await client.SendAsync(primeRequest);

        // Now the next request should attach the header
        mock.SetResponse(HttpStatusCode.OK);
        using var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/rpc");

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        await Assert.That(mock.LastRequest!.Headers.TryGetValues(SessionIdHeader, out var values)).IsTrue();
        await Assert.That(values!.First()).IsEqualTo("initial-session");
    }

    [Test]
    public async Task SendAsync_Response409WithHeader_UpdatesStoredSessionId()
    {
        // Arrange
        var (handler, mock) = CreatePipeline();
        mock.SetResponse(HttpStatusCode.Conflict, sessionId: "new-session-id");

        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/rpc");

        // Act
        using var response = await client.SendAsync(request);

        // Assert — send second request and verify the updated ID is attached
        mock.SetResponse(HttpStatusCode.OK);
        using var request2 = new HttpRequestMessage(HttpMethod.Post, "http://localhost/rpc");
        using var response2 = await client.SendAsync(request2);

        await Assert.That(mock.LastRequest!.Headers.TryGetValues(SessionIdHeader, out var values)).IsTrue();
        await Assert.That(values!.First()).IsEqualTo("new-session-id");
    }

    [Test]
    public async Task SendAsync_Response409WithoutHeader_DoesNotChangeStoredSessionId()
    {
        // Arrange
        var (handler, mock) = CreatePipeline();

        // Prime with a known session ID
        mock.SetResponse(HttpStatusCode.Conflict, sessionId: "original-session");
        using var client = new HttpClient(handler);
        using var prime = new HttpRequestMessage(HttpMethod.Post, "http://localhost/rpc");
        using var _ = await client.SendAsync(prime);

        // Now send 409 without a session header
        mock.SetResponse(HttpStatusCode.Conflict, sessionId: null);
        using var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/rpc");
        using var response = await client.SendAsync(request);

        // Act — the next request should still use the original session ID
        mock.SetResponse(HttpStatusCode.OK);
        using var request2 = new HttpRequestMessage(HttpMethod.Post, "http://localhost/rpc");
        using var response2 = await client.SendAsync(request2);

        // Assert
        await Assert.That(mock.LastRequest!.Headers.TryGetValues(SessionIdHeader, out var values)).IsTrue();
        await Assert.That(values!.First()).IsEqualTo("original-session");
    }

    [Test]
    public async Task SendAsync_Response200_DoesNotChangeStoredSessionId()
    {
        // Arrange
        var (handler, mock) = CreatePipeline();

        // Prime with a known session ID
        mock.SetResponse(HttpStatusCode.Conflict, sessionId: "original-session");
        using var client = new HttpClient(handler);
        using var prime = new HttpRequestMessage(HttpMethod.Post, "http://localhost/rpc");
        using var _ = await client.SendAsync(prime);

        // Send a 200 response (no session header)
        mock.SetResponse(HttpStatusCode.OK);
        using var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/rpc");
        using var response = await client.SendAsync(request);

        // Act — next request should still carry the original session ID
        mock.SetResponse(HttpStatusCode.OK);
        using var request2 = new HttpRequestMessage(HttpMethod.Post, "http://localhost/rpc");
        using var response2 = await client.SendAsync(request2);

        // Assert
        await Assert.That(mock.LastRequest!.Headers.TryGetValues(SessionIdHeader, out var values)).IsTrue();
        await Assert.That(values!.First()).IsEqualTo("original-session");
    }
}

/// <summary>
/// A simple fake <see cref="HttpMessageHandler"/> for testing.
/// </summary>
internal sealed class MockHttpMessageHandler : HttpMessageHandler
{
    private HttpStatusCode _statusCode = HttpStatusCode.OK;
    private string? _sessionId;

    public HttpRequestMessage? LastRequest { get; private set; }

    public void SetResponse(HttpStatusCode statusCode, string? sessionId = null)
    {
        _statusCode = statusCode;
        _sessionId = sessionId;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        LastRequest = request;
        var response = new HttpResponseMessage(_statusCode);

        if (_sessionId is not null)
        {
            response.Headers.TryAddWithoutValidation("X-Transmission-Session-Id", _sessionId);
        }

        return Task.FromResult(response);
    }
}
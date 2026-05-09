using Microsoft.Extensions.Logging;

namespace TransmissionHub.Client.Internal.Http;

/// <summary>
/// A delegating handler responsible for attaching and updating
/// the <c>X-Transmission-Session-Id</c> header on every request.
/// </summary>
/// <remarks>
/// This handler has a single responsibility: session header management.
/// The retry policy on 409 Conflict is handled separately via
/// <c>AddResilienceHandler</c> in the HTTP client pipeline.
/// <para>
/// Pipeline order: <c>ResilienceHandler</c> → <c>SessionIdHandler</c> → <c>HttpClientHandler</c>.
/// When a 409 is received, this handler updates the stored session ID from the response header.
/// The outer <c>ResilienceHandler</c> then retries the request, at which point this handler
/// attaches the updated session ID automatically.
/// </para>
/// </remarks>
internal sealed partial class SessionIdDelegatingHandler : DelegatingHandler
{
    private const string SessionIdHeader = "X-Transmission-Session-Id";

    private readonly ILogger<SessionIdDelegatingHandler> _logger;
    private volatile string? _sessionId;

    public SessionIdDelegatingHandler(ILogger<SessionIdDelegatingHandler> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        AttachSessionId(request);

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            UpdateSessionId(response);
        }

        return response;
    }

    private void AttachSessionId(HttpRequestMessage request)
    {
        var sessionId = _sessionId;
        if (sessionId is null)
        {
            return;
        }

        request.Headers.Remove(SessionIdHeader);
        request.Headers.TryAddWithoutValidation(SessionIdHeader, sessionId);

        LogSessionIdAttached(sessionId);
    }

    private void UpdateSessionId(HttpResponseMessage response)
    {
        if (response.Headers.TryGetValues(SessionIdHeader, out var values))
        {
            var newSessionId = values.FirstOrDefault();
            if (newSessionId is not null)
            {
                _sessionId = newSessionId;
                LogSessionIdUpdated(newSessionId);
                return;
            }
        }

        LogSessionIdMissingIn409();
    }

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Attaching session ID '{SessionId}' to request.")]
    private partial void LogSessionIdAttached(string sessionId);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "409 Conflict received. Session ID updated to '{SessionId}'.")]
    private partial void LogSessionIdUpdated(string sessionId);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "409 Conflict received but response did not contain a new session ID header.")]
    private partial void LogSessionIdMissingIn409();
}
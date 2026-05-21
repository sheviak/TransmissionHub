using System.Net;
using System.Net.Mime;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using TransmissionHub.Client.Internal.Clients;
using TransmissionHub.Client.Internal.Dialects;
using TransmissionHub.Client.Models.Enums;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Tests.Internal.Clients;

public class TransmissionClientV16Tests
{
    private class MockHttpMessageHandler(string responseContent, HttpStatusCode statusCode = HttpStatusCode.OK)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(responseContent, Encoding.UTF8, MediaTypeNames.Application.Json)
            });
        }
    }

    [Test]
    public async Task TorrentGetAsync_ReturnsResultOk_WithCorrectTorrents()
    {
        // Arrange
        const string responseJson =
            """
            {
                "arguments": {
                    "torrents": [
                        {
                            "id": 1,
                            "status": 6,
                            "percentDone": 1,
                            "leftUntilDone": 0,
                            "name": "Movie.mkv",
                            "totalSize": 13408738957,
                            "hashString": "0547e99242ced14ae89b3516ef7d7dd3cf3c0541"
                        }
                    ]
                },
                "result": "success"
            }
            """;

        var httpClient = new HttpClient(new MockHttpMessageHandler(responseJson));
        var dialect = new RpcRequestDialect();
        var options = new TransmissionClientOptions
        {
            Url = new Uri("http://localhost:9091/transmission/rpc"),
            DownloadDirectory = "/downloads",
        };

        var client = new TransmissionClientV16(httpClient, dialect, options, NullLogger<TransmissionClientV16>.Instance);

        var request = new TorrentGetRequest
        {
            Fields =
            [
                TorrentGetRequest.TorrentFields.Id,
                TorrentGetRequest.TorrentFields.Name,
                TorrentGetRequest.TorrentFields.Status,
                TorrentGetRequest.TorrentFields.TotalSize,
                TorrentGetRequest.TorrentFields.HashString,
                TorrentGetRequest.TorrentFields.PercentDone,
                TorrentGetRequest.TorrentFields.LeftUntilDone,
            ]
        };

        // Act
        var result = await client.TorrentGetAsync(request);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value).IsNotNull();
        await Assert.That(result.Value.Torrents).IsNotNull();
        await Assert.That(result.Value.Torrents).Count().IsEqualTo(1);

        var torrent = result.Value.Torrents![0];

        await Assert.That(torrent.Id).IsEqualTo(1);
        await Assert.That(torrent.PercentDone).IsEqualTo(1);
        await Assert.That(torrent.LeftUntilDone).IsEqualTo(0);
        await Assert.That(torrent.Name).IsEqualTo("Movie.mkv");
        await Assert.That(torrent.TotalSize).IsEqualTo(13408738957);
        await Assert.That(torrent.Status).IsEqualTo(TorrentStatus.Seed);
        await Assert.That(torrent.HashString).IsEqualTo("0547e99242ced14ae89b3516ef7d7dd3cf3c0541");
    }
}
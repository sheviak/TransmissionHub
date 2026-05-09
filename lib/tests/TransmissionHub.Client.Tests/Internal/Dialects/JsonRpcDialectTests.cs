using System.Text.Json;
using TransmissionHub.Client.Internal.Dialects;
using TransmissionHub.Client.Models;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Tests.Internal.Dialects;

public class JsonRpcDialectTests
{
    private readonly JsonRpcDialect _dialect = new();

    // =========================================================================
    // SerializeRequest — envelope structure and method format
    // =========================================================================

    [Test]
    public async Task SerializeRequest_ProducesJsonRpc20Envelope()
    {
        // Verify the JSON-RPC 2.0 envelope format: { "jsonrpc": "2.0", "method": "...", "params": ... }
        var json = _dialect.SerializeRequest(RpcMethod.TorrentGet, new { ids = new[] { 1 } });
        using var doc = JsonDocument.Parse(json);

        await Assert.That(doc.RootElement.GetProperty("jsonrpc").GetString()).IsEqualTo("2.0");
        await Assert.That(doc.RootElement.TryGetProperty("method", out _)).IsTrue();
        await Assert.That(doc.RootElement.TryGetProperty("params", out _)).IsTrue();
        // Must NOT contain legacy envelope fields
        await Assert.That(doc.RootElement.TryGetProperty("arguments", out _)).IsFalse();
    }

    [Test]
    public async Task SerializeRequest_MethodName_IsSnakeCase()
    {
        var json = _dialect.SerializeRequest(RpcMethod.TorrentGet, null);
        using var doc = JsonDocument.Parse(json);

        await Assert.That(doc.RootElement.GetProperty("method").GetString()).IsEqualTo("torrent_get");
    }

    [Test]
    public async Task SerializeRequest_NullArguments_OmitsParamsKey()
    {
        var json = _dialect.SerializeRequest(RpcMethod.SessionStats, null);
        using var doc = JsonDocument.Parse(json);

        await Assert.That(doc.RootElement.TryGetProperty("params", out _)).IsFalse();
    }

    // =========================================================================
    // SerializeRequest — real request serialized as snake_case
    // =========================================================================

    [Test]
    public async Task SerializeRequest_FreeSpaceRequest_ParamsUseSnakeCase()
    {
        var request = new FreeSpaceRequest { Path = "/downloads" };

        var json = _dialect.SerializeRequest(RpcMethod.FreeSpace, request);
        using var doc = JsonDocument.Parse(json);
        var @params = doc.RootElement.GetProperty("params");

        await Assert.That(@params.GetProperty("path").GetString()).IsEqualTo("/downloads");
    }

    [Test]
    public async Task SerializeRequest_TorrentGet_FieldsArrayContainsSnakeCaseWireNames()
    {
        // In JSON-RPC 2.0 dialect, ALL fields use snake_case — no exceptions.
        var wireFields = _dialect.NormalizeFields([
            TorrentGetRequest.TorrentFields.Id,          // "Id"         → "id"
            TorrentGetRequest.TorrentFields.Name,        // "Name"       → "name"
            TorrentGetRequest.TorrentFields.FileCount,   // "FileCount"   → "file_count" (no kebab here)
            TorrentGetRequest.TorrentFields.DownloadDir, // "DownloadDir" → "download_dir"
            TorrentGetRequest.TorrentFields.PeerLimit,   // "PeerLimit"   → "peer_limit"
        ]);

        var request = new TorrentGetRequest
        {
            Ids = [new TorrentId(1)],
            Fields = wireFields,
        };

        var json = _dialect.SerializeRequest(RpcMethod.TorrentGet, request);
        using var doc = JsonDocument.Parse(json);
        var fields = doc.RootElement
            .GetProperty("params")
            .GetProperty("fields")
            .EnumerateArray()
            .Select(e => e.GetString()!)
            .ToList();

        await Assert.That(fields).Contains("id");
        await Assert.That(fields).Contains("name");
        await Assert.That(fields).Contains("file_count");    // snake_case (NOT file-count)
        await Assert.That(fields).Contains("download_dir");  // snake_case (NOT downloadDir)
        await Assert.That(fields).Contains("peer_limit");    // snake_case (NOT peer-limit)
    }

    // =========================================================================
    // NormalizeFields — all fields become snake_case uniformly
    // =========================================================================

    [Test]
    public async Task NormalizeFields_AnyField_ProducesSnakeCase()
    {
        // Unlike legacy dialect, all fields use the same policy — no exceptions
        var result = _dialect.NormalizeFields(["FileCount", "PeerLimit", "ActivityDate", "HashString"]);

        await Assert.That(result[0]).IsEqualTo("file_count");
        await Assert.That(result[1]).IsEqualTo("peer_limit");
        await Assert.That(result[2]).IsEqualTo("activity_date");
        await Assert.That(result[3]).IsEqualTo("hash_string");
    }

    // =========================================================================
    // ExtractPayload — JSON-RPC 2.0 success / error
    // =========================================================================

    [Test]
    public async Task ExtractPayload_SuccessResponse_ReturnsResultPayload()
    {
        const string rawJson = """{"jsonrpc":"2.0","result":{"path":"/downloads","size_bytes":1024},"id":1}""";

        var result = _dialect.ExtractPayload(rawJson);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.TryGetProperty("size_bytes", out _)).IsTrue();
    }

    [Test]
    public async Task ExtractPayload_ErrorResponse_ReturnsFailureWithCodeAndMessage()
    {
        const string rawJson = """{"jsonrpc":"2.0","error":{"code":-32600,"message":"Invalid request"},"id":1}""";

        var result = _dialect.ExtractPayload(rawJson);

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsEqualTo("Invalid request");
        await Assert.That(result.Error!.Value.Code).IsEqualTo(-32600);
    }

    // =========================================================================
    // Deserialize — pure snake_case, no normalization step
    // =========================================================================

    [Test]
    public async Task Deserialize_SnakeCasePayload_MapsToRecord()
    {
        const string payloadJson = """{"size_bytes":5000000,"total_size":10000000,"path":"/downloads"}""";
        var payload = JsonDocument.Parse(payloadJson).RootElement;

        var result = _dialect.Deserialize<FreeSpaceSnakeModel>(payload);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.SizeBytes).IsEqualTo(5_000_000L);
        await Assert.That(result.Value.TotalSize).IsEqualTo(10_000_000L);
        await Assert.That(result.Value.Path).IsEqualTo("/downloads");
    }

    [Test]
    public async Task Deserialize_NestedSnakeCasePayload_MapsRecursively()
    {
        const string payloadJson = """
        {
          "active_torrent_count": 5,
          "cumulative_stats": { "uploaded_bytes": 1000, "downloaded_bytes": 2000 },
          "current_stats":    { "uploaded_bytes": 100,  "downloaded_bytes": 200 }
        }
        """;
        var payload = JsonDocument.Parse(payloadJson).RootElement;

        var result = _dialect.Deserialize<SessionStatsSnakeModel>(payload);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.ActiveTorrentCount).IsEqualTo(5);
        await Assert.That(result.Value.CumulativeStats!.UploadedBytes).IsEqualTo(1000);
        await Assert.That(result.Value.CurrentStats!.DownloadedBytes).IsEqualTo(200);
    }
}

// ---------------------------------------------------------------------------
// Local test models
// ---------------------------------------------------------------------------

file record FreeSpaceSnakeModel
{
    public string? Path { get; init; }
    public long SizeBytes { get; init; }
    public long TotalSize { get; init; }
}

file record SessionStatsSnakeModel
{
    public int ActiveTorrentCount { get; init; }
    public SessionStatsSnakeEntryModel? CumulativeStats { get; init; }
    public SessionStatsSnakeEntryModel? CurrentStats { get; init; }
}

file record SessionStatsSnakeEntryModel
{
    public long UploadedBytes { get; init; }
    public long DownloadedBytes { get; init; }
}
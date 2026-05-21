using System.Text.Json;
using TransmissionHub.Client.Internal.Dialects;
using TransmissionHub.Client.Models;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Tests.Internal.Dialects;

public class RpcRequestDialectTests
{
    private readonly RpcRequestDialect _dialect = new();

    // =========================================================================
    // SerializeRequest — envelope structure and method format
    // =========================================================================

    [Test]
    public async Task SerializeRequest_ProducesLegacyEnvelope()
    {
        // Arrange
        var requestArgs = new { ids = new[] { 1, 2 } };

        // Act
        var json = _dialect.SerializeRequest(RpcMethod.TorrentGet, requestArgs);
        using var doc = JsonDocument.Parse(json.Value);

        // Assert
        // Verify the legacy envelope format: { "method": "...", "arguments": ... }
        await Assert.That(doc.RootElement.TryGetProperty("method", out _)).IsTrue();
        await Assert.That(doc.RootElement.TryGetProperty("arguments", out _)).IsTrue();
        // Must NOT contain JSON-RPC 2.0 fields
        await Assert.That(doc.RootElement.TryGetProperty("jsonrpc", out _)).IsFalse();
        await Assert.That(doc.RootElement.TryGetProperty("params", out _)).IsFalse();
    }

    [Test]
    public async Task SerializeRequest_MethodName_IsKebabCase()
    {
        // Act
        var json = _dialect.SerializeRequest(RpcMethod.TorrentGet, null);
        using var doc = JsonDocument.Parse(json.Value);

        // Assert
        await Assert.That(doc.RootElement.GetProperty("method").GetString()).IsEqualTo("torrent-get");
    }

    [Test]
    public async Task SerializeRequest_NullArguments_OmitsArgumentsKey()
    {
        // Act
        var json = _dialect.SerializeRequest(RpcMethod.SessionStats, null);
        using var doc = JsonDocument.Parse(json.Value);

        // Assert
        await Assert.That(doc.RootElement.TryGetProperty("arguments", out _)).IsFalse();
    }

    // =========================================================================
    // SerializeRequest — real TorrentGetRequest with Fields including kebab names
    // =========================================================================

    [Test]
    public async Task SerializeRequest_TorrentGet_FieldsArrayContainsLegacyWireNames()
    {
        // Arrange
        // NormalizeFields converts PascalCase → legacy wire names (kebab or camelCase).
        // The caller is responsible for normalizing fields before passing them to the request.
        var wireFields = _dialect.NormalizeFields([
            TorrentGetRequest.TorrentFields.Id,              // "Id"            → "id"           (camelCase fallback)
            TorrentGetRequest.TorrentFields.Name,            // "Name"          → "name"         (camelCase fallback)
            TorrentGetRequest.TorrentFields.FileCount,       // "FileCount"      → "file-count"   (kebab override)
            TorrentGetRequest.TorrentFields.DownloadDir,     // "DownloadDir"    → "download-dir" (kebab override)
            TorrentGetRequest.TorrentFields.PeerLimit,       // "PeerLimit"      → "peer-limit"   (kebab override)
            TorrentGetRequest.TorrentFields.SeedRatioLimit,  // "SeedRatioLimit" → "seedRatioLimit" (camelCase fallback)
        ]);

        var request = new TorrentGetRequest
        {
            Ids = [new TorrentId(7)],
            Fields = wireFields,
        };

        // Act
        var json = _dialect.SerializeRequest(RpcMethod.TorrentGet, request);
        using var doc = JsonDocument.Parse(json.Value);
        var fields = doc.RootElement
            .GetProperty("arguments")
            .GetProperty("fields")
            .EnumerateArray()
            .Select(e => e.GetString()!)
            .ToList();

        // Assert
        await Assert.That(fields).Contains("id");
        await Assert.That(fields).Contains("name");
        await Assert.That(fields).Contains("file-count");       // kebab override
        await Assert.That(fields).Contains("download-dir");     // kebab override (NOT downloadDir)
        await Assert.That(fields).Contains("peer-limit");       // kebab override
        await Assert.That(fields).Contains("seedRatioLimit");   // camelCase fallback
    }

    // =========================================================================
    // NormalizeFields — specific kebab exceptions vs camelCase fallback
    // =========================================================================

    [Test]
    public async Task NormalizeFields_MixOfCamelAndKebabFields_NormalizesCorrectly()
    {
        // Arrange
        // file-count and peer-limit are the most important kebab exceptions in torrent-get
        var fields = new[] { "Name", "FileCount", "PeerLimit", "HashString" };

        // Act
        var result = _dialect.NormalizeFields(fields);

        // Assert
        await Assert.That(result[0]).IsEqualTo("name");           // camelCase fallback
        await Assert.That(result[1]).IsEqualTo("file-count");     // kebab override
        await Assert.That(result[2]).IsEqualTo("peer-limit");     // kebab override
        await Assert.That(result[3]).IsEqualTo("hashString");     // camelCase fallback
    }

    // =========================================================================
    // ExtractPayload — success / failure
    // =========================================================================

    [Test]
    public async Task ExtractPayload_SuccessResponse_ReturnsArgumentsPayload()
    {
        // Arrange
        const string rawJson = """{"result":"success","arguments":{"path":"/downloads","size-bytes":1024}}""";

        // Act
        var result = _dialect.ExtractPayload(rawJson);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.TryGetProperty("size-bytes", out _)).IsTrue();
    }

    [Test]
    public async Task ExtractPayload_ErrorResponse_ReturnsFailureWithMessage()
    {
        // Arrange
        const string rawJson = """{"result":"no such torrent","arguments":{}}""";

        // Act
        var result = _dialect.ExtractPayload(rawJson);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsEqualTo("no such torrent");
    }

    // =========================================================================
    // Deserialize — key normalization: camelCase + kebab → snake_case → T
    // =========================================================================

    [Test]
    public async Task Deserialize_MixedLegacyKeys_MapsToRecord()
    {
        // Arrange
        // Realistic free-space response with mixed key styles:
        // "size-bytes" (kebab), "total_size" (underscore anomaly), "path" (plain)
        const string payloadJson = """{"size-bytes":5000000,"total_size":10000000,"path":"/downloads"}""";
        var payload = JsonDocument.Parse(payloadJson).RootElement;

        // Act
        var result = _dialect.Deserialize<FreeSpaceTestModel>(payload);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.SizeBytes).IsEqualTo(5_000_000L);
        await Assert.That(result.Value.TotalSize).IsEqualTo(10_000_000L);
        await Assert.That(result.Value.Path).IsEqualTo("/downloads");
    }

    [Test]
    public async Task Deserialize_SessionStatsWithKebabNestedKeys_MapsNestedObjects()
    {
        // Arrange
        // "cumulative-stats" and "current-stats" are kebab-case top-level keys in session-stats
        const string payloadJson = """
        {
          "activeTorrentCount": 2,
          "cumulative-stats": { "uploadedBytes": 100, "downloadedBytes": 200 },
          "current-stats":    { "uploadedBytes": 10,  "downloadedBytes": 20 }
        }
        """;
        var payload = JsonDocument.Parse(payloadJson).RootElement;

        // Act
        var result = _dialect.Deserialize<SessionStatsTestModel>(payload);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.ActiveTorrentCount).IsEqualTo(2);
        await Assert.That(result.Value.CumulativeStats).IsNotNull();
        await Assert.That(result.Value.CumulativeStats!.UploadedBytes).IsEqualTo(100);
        await Assert.That(result.Value.CurrentStats!.DownloadedBytes).IsEqualTo(20);
    }
}

// ---------------------------------------------------------------------------
// Local test models
// ---------------------------------------------------------------------------

file record FreeSpaceTestModel
{
    public string? Path { get; init; }
    public long SizeBytes { get; init; }
    public long TotalSize { get; init; }
}

file record SessionStatsTestModel
{
    public int ActiveTorrentCount { get; init; }
    public SessionStatsEntryTestModel? CumulativeStats { get; init; }
    public SessionStatsEntryTestModel? CurrentStats { get; init; }
}

file record SessionStatsEntryTestModel
{
    public long UploadedBytes { get; init; }
    public long DownloadedBytes { get; init; }
}
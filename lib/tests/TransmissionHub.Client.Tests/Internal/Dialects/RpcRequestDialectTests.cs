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
        // NormalizeFields converts PascalCase → legacy wire names per the torrent-get
        // field table (camelCase with kebab exceptions). The caller is responsible
        // for normalizing fields before passing them to the request.
        var wireFields = _dialect.NormalizeFields(RpcMethod.TorrentGet, [
            TorrentGetRequest.TorrentFields.Id,              // "Id"            → "id"             (camelCase fallback)
            TorrentGetRequest.TorrentFields.Name,            // "Name"          → "name"           (camelCase fallback)
            TorrentGetRequest.TorrentFields.FileCount,       // "FileCount"      → "file-count"     (kebab override)
            TorrentGetRequest.TorrentFields.DownloadDir,     // "DownloadDir"    → "downloadDir"    (camelCase override)
            TorrentGetRequest.TorrentFields.PeerLimit,       // "PeerLimit"      → "peer-limit"     (kebab override)
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
        await Assert.That(fields).Contains("downloadDir");      // camelCase override (NOT download-dir)
        await Assert.That(fields).Contains("peer-limit");       // kebab override
        await Assert.That(fields).Contains("seedRatioLimit");   // camelCase fallback
    }

    // =========================================================================
    // NormalizeFields — specific kebab exceptions vs camelCase fallback
    // =========================================================================

    [Test]
    public async Task NormalizeFields_TorrentGet_MixOfCamelAndKebabFields_NormalizesCorrectly()
    {
        // Arrange
        // file-count and peer-limit are the most important kebab exceptions in torrent-get
        var fields = new[] { "Name", "FileCount", "PeerLimit", "HashString" };

        // Act
        var result = _dialect.NormalizeFields(RpcMethod.TorrentGet, fields);

        // Assert
        await Assert.That(result[0]).IsEqualTo("name");           // camelCase fallback
        await Assert.That(result[1]).IsEqualTo("file-count");     // kebab override
        await Assert.That(result[2]).IsEqualTo("peer-limit");     // kebab override
        await Assert.That(result[3]).IsEqualTo("hashString");     // camelCase fallback
    }

    [Test]
    public async Task NormalizeFields_SessionGet_DownloadDir_IsKebab()
    {
        // Arrange / Act
        var result = _dialect.NormalizeFields(RpcMethod.SessionGet, new[]
        {
            "DownloadDir",      // "DownloadDir" → "download-dir" in the session scope
            "AltSpeedDown",     // "AltSpeedDown" → "alt-speed-down"
            "Encryption",       // "Encryption"   → "encryption" (camelCase fallback)
        });

        // Assert
        await Assert.That(result[0]).IsEqualTo("download-dir");   // session override (NOT downloadDir)
        await Assert.That(result[1]).IsEqualTo("alt-speed-down"); // session override
        await Assert.That(result[2]).IsEqualTo("encryption");     // camelCase fallback
    }

    [Test]
    public async Task NormalizeFields_EmptyFields_ReturnsNull()
    {
        await Assert.That(_dialect.NormalizeFields(RpcMethod.TorrentGet, [])).IsNull();
        await Assert.That(_dialect.NormalizeFields(RpcMethod.TorrentGet, null)).IsNull();
    }

    // =========================================================================
    // SerializeRequest — argument bodies use legacy wire names
    // =========================================================================
    // The server matches argument keys with exact, case-sensitive byte comparison
    // (tr_quark_lookup), so a camelCase key such as "deleteLocalData" would be
    // silently ignored — every argument key must be rewritten to the wire format.

    [Test]
    public async Task SerializeRequest_TorrentAdd_ArgumentKeys_UseLegacyWireNames()
    {
        // Arrange
        var request = new TorrentAddRequest
        {
            DownloadDir = "/downloads",
            PeerLimit = 10,
            FilesWanted = [1, 2],
            Paused = true,
        };

        // Act
        var json = _dialect.SerializeRequest(RpcMethod.TorrentAdd, request);
        using var doc = JsonDocument.Parse(json.Value);
        var args = doc.RootElement.GetProperty("arguments");

        // Assert
        await Assert.That(args.GetProperty("download-dir").GetString()).IsEqualTo("/downloads");
        await Assert.That(args.GetProperty("peer-limit").GetInt32()).IsEqualTo(10);
        await Assert.That(args.GetProperty("files-wanted").EnumerateArray().Select(e => e.GetInt32())).IsEquivalentTo([1, 2]);
        await Assert.That(args.GetProperty("paused").GetBoolean()).IsTrue();
        // And none of the unconverted PascalCase/camelCase keys remain
        await Assert.That(args.TryGetProperty("downloadDir", out _)).IsFalse();
        await Assert.That(args.TryGetProperty("peerLimit", out _)).IsFalse();
        await Assert.That(args.TryGetProperty("filesWanted", out _)).IsFalse();
    }

    [Test]
    public async Task SerializeRequest_TorrentRemove_ArgumentKeys_UseLegacyWireNames()
    {
        // Arrange
        var request = new TorrentRemoveRequest
        {
            Ids = [new TorrentId(1)],
            DeleteLocalData = true,
        };

        // Act
        var json = _dialect.SerializeRequest(RpcMethod.TorrentRemove, request);
        using var doc = JsonDocument.Parse(json.Value);
        var args = doc.RootElement.GetProperty("arguments");

        // Assert
        await Assert.That(args.TryGetProperty("delete-local-data", out var flag)).IsTrue();
        await Assert.That(flag.GetBoolean()).IsTrue();
        await Assert.That(args.TryGetProperty("deleteLocalData", out _)).IsFalse();
    }

    [Test]
    public async Task SerializeRequest_TorrentSet_ArgumentKeys_UseLegacyWireNames()
    {
        // Arrange
        var request = new TorrentSetRequest
        {
            Ids = [new TorrentId(3)],
            PeerLimit = 5,
            FilesWanted = [1],
            PriorityHigh = [2],
        };

        // Act
        var json = _dialect.SerializeRequest(RpcMethod.TorrentSet, request);
        using var doc = JsonDocument.Parse(json.Value);
        var args = doc.RootElement.GetProperty("arguments");

        // Assert
        await Assert.That(args.GetProperty("peer-limit").GetInt32()).IsEqualTo(5);
        await Assert.That(args.GetProperty("files-wanted").EnumerateArray().Single().GetInt32()).IsEqualTo(1);
        await Assert.That(args.GetProperty("priority-high").EnumerateArray().Single().GetInt32()).IsEqualTo(2);
        await Assert.That(args.GetProperty("ids").EnumerateArray().Single().GetInt32()).IsEqualTo(3); // camelCase fallback
    }

    [Test]
    public async Task SerializeRequest_SessionSet_ArgumentKeys_UseLegacyWireNames()
    {
        // Arrange
        var request = new SessionSetRequest
        {
            AltSpeedDown = 100,
            DownloadDir = "/downloads",
            PeerPort = 51413,
        };

        // Act
        var json = _dialect.SerializeRequest(RpcMethod.SessionSet, request);
        using var doc = JsonDocument.Parse(json.Value);
        var args = doc.RootElement.GetProperty("arguments");

        // Assert
        await Assert.That(args.GetProperty("alt-speed-down").GetInt32()).IsEqualTo(100);
        await Assert.That(args.GetProperty("download-dir").GetString()).IsEqualTo("/downloads");
        await Assert.That(args.GetProperty("peer-port").GetInt32()).IsEqualTo(51413);
        await Assert.That(args.TryGetProperty("downloadDir", out _)).IsFalse();
    }

    [Test]
    public async Task SerializeRequest_GroupSet_ArgumentKeys_UseLegacyWireNames()
    {
        // Arrange
        var request = new GroupSetRequest
        {
            Name = "slow",
            SpeedLimitDown = 1024,
            SpeedLimitDownEnabled = true,
            SpeedLimitUpEnabled = true,
        };

        // Act
        var json = _dialect.SerializeRequest(RpcMethod.GroupSet, request);
        using var doc = JsonDocument.Parse(json.Value);
        var args = doc.RootElement.GetProperty("arguments");

        // Assert
        await Assert.That(args.GetProperty("name").GetString()).IsEqualTo("slow");               // camelCase fallback
        await Assert.That(args.GetProperty("speed-limit-down").GetInt32()).IsEqualTo(1024);      // session override
        await Assert.That(args.GetProperty("speed-limit-down-enabled").GetBoolean()).IsTrue();   // session override
    }

    [Test]
    public async Task SerializeRequest_NestedArgumentObject_RewritesKeysRecursively()
    {
        // Arrange
        // Request models are flat, but SerializeRequest accepts arbitrary object graphs
        // (e.g. group-set in JSON-RPC) — key rewriting must recurse into nested objects
        // and into objects inside arrays.
        var arguments = new
        {
            Ids = new[] { 1 },
            Options = new { FilesWanted = new[] { 2 } },
            Nested = new[] { new { DeleteLocalData = true } },
        };

        // Act
        var json = _dialect.SerializeRequest(RpcMethod.TorrentAdd, arguments);
        using var doc = JsonDocument.Parse(json.Value);
        var args = doc.RootElement.GetProperty("arguments");

        // Assert
        await Assert.That(args.GetProperty("options").GetProperty("files-wanted").EnumerateArray().Single().GetInt32()).IsEqualTo(2);
        await Assert.That(args.GetProperty("nested").EnumerateArray().Single().GetProperty("delete-local-data").GetBoolean()).IsTrue();
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
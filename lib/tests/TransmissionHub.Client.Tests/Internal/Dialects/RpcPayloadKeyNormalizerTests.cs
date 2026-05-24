using System.Text.Json;
using TransmissionHub.Client.Internal.Dialects;

namespace TransmissionHub.Client.Tests.Internal.Dialects;

public class RpcPayloadKeyNormalizerTests
{
    // Helper: normalize raw JSON and return the root element for assertions.
    private static JsonElement Normalize(string json)
    {
        var element = JsonDocument.Parse(json).RootElement;
        var bytes = RpcPayloadKeyNormalizer.NormalizeJsonToBytes(element).ToArray();
        return JsonDocument.Parse(bytes).RootElement;
    }

    // =========================================================================
    // ToSnakeCase — unit tests for the core conversion logic
    // =========================================================================

    [Test]
    public async Task ToSnakeCase_CamelCase_InsertsUnderscores()
    {
        // Arrange
        var buf = new char[50];

        // Act
        var n = RpcPayloadKeyNormalizer.ToSnakeCase("activeTorrentCount", buf);

        // Assert
        await Assert.That(new string(buf[..n])).IsEqualTo("active_torrent_count");
    }

    [Test]
    public async Task ToSnakeCase_PascalCase_LowercasesFirst()
    {
        // Arrange
        var buf = new char[50];

        // Act
        var n = RpcPayloadKeyNormalizer.ToSnakeCase("HashString", buf);

        // Assert
        await Assert.That(new string(buf[..n])).IsEqualTo("hash_string");
    }

    [Test]
    public async Task ToSnakeCase_KebabCase_ReplacesHyphens()
    {
        // Arrange
        var buf = new char[50];

        // Act
        var n = RpcPayloadKeyNormalizer.ToSnakeCase("cumulative-stats", buf);

        // Assert
        await Assert.That(new string(buf[..n])).IsEqualTo("cumulative_stats");
    }

    [Test]
    public async Task ToSnakeCase_AlreadySnakeCase_IsIdempotent()
    {
        // Arrange
        var buf = new char[50];

        // Act
        var n = RpcPayloadKeyNormalizer.ToSnakeCase("total_size", buf);

        // Assert
        await Assert.That(new string(buf[..n])).IsEqualTo("total_size");
    }

    [Test]
    public async Task ToSnakeCase_SingleWord_Lowercased()
    {
        // Arrange
        var buf = new char[20];

        // Act
        var n = RpcPayloadKeyNormalizer.ToSnakeCase("Name", buf);

        // Assert
        await Assert.That(new string(buf[..n])).IsEqualTo("name");
    }

    // =========================================================================
    // NormalizeJsonToBytes — key format variants
    // =========================================================================

    [Test]
    public async Task NormalizeJsonToBytes_CamelCaseKey_ConvertsToSnakeCase()
    {
        // Arrange & Act
        var result = Normalize("""{"activeTorrentCount":5}""");

        // Assert
        await Assert.That(result.TryGetProperty("active_torrent_count", out _)).IsTrue();
    }

    [Test]
    public async Task NormalizeJsonToBytes_KebabCaseKey_ConvertsToSnakeCase()
    {
        // Arrange & Act
        var result = Normalize("""{"cumulative-stats":100,"size-bytes":1024}""");

        // Assert
        await Assert.That(result.TryGetProperty("cumulative_stats", out _)).IsTrue();
        await Assert.That(result.TryGetProperty("size_bytes", out _)).IsTrue();
    }

    [Test]
    public async Task NormalizeJsonToBytes_SnakeCaseKey_IsIdempotent()
    {
        // Arrange & Act
        var result = Normalize("""{"total_size":5000000}""");

        // Assert
        await Assert.That(result.TryGetProperty("total_size", out var v)).IsTrue();
        await Assert.That(v.GetInt64()).IsEqualTo(5_000_000L);
    }

    [Test]
    public async Task NormalizeJsonToBytes_PascalCaseKey_ConvertsToSnakeCase()
    {
        // Arrange & Act
        var result = Normalize("""{"HashString":"abc","Name":"test"}""");

        // Assert
        await Assert.That(result.TryGetProperty("hash_string", out _)).IsTrue();
        await Assert.That(result.TryGetProperty("name", out _)).IsTrue();
    }

    [Test]
    public async Task NormalizeJsonToBytes_MixedFormats_AllConvertToSnakeCase()
    {
        // Arrange & Act
        // Realistic session-stats payload: camelCase + kebab-case + snake_case
        const string json = """
        {
          "activeTorrentCount": 2,
          "cumulative-stats": { "uploaded-bytes": 100 },
          "current-stats":    { "downloadedBytes": 50 },
          "total_size": 5000000
        }
        """;

        // Act
        var result = Normalize(json);

        // Assert
        await Assert.That(result.TryGetProperty("active_torrent_count", out _)).IsTrue();
        await Assert.That(result.TryGetProperty("cumulative_stats", out _)).IsTrue();
        await Assert.That(result.TryGetProperty("current_stats", out _)).IsTrue();
        await Assert.That(result.TryGetProperty("total_size", out _)).IsTrue();
    }

    [Test]
    public async Task NormalizeJsonToBytes_NestedObject_NormalizesRecursively()
    {
        // Arrange & Act
        // Nested camelCase keys inside kebab-case parent
        const string json = """{"cumulative-stats":{"uploadedBytes":100,"downloadedBytes":200}}""";

        // Act
        var result = Normalize(json);
        var nested = result.GetProperty("cumulative_stats");

        // Assert
        await Assert.That(nested.TryGetProperty("uploaded_bytes", out _)).IsTrue();
        await Assert.That(nested.TryGetProperty("downloaded_bytes", out _)).IsTrue();
    }

    [Test]
    public async Task NormalizeJsonToBytes_ArrayOfObjects_NormalizesEachElement()
    {
        // Arrange & Act
        const string json = """{"trackerList":[{"announceUrl":"http://a.b"},{"announceUrl":"http://c.d"}]}""";

        // Act
        var result = Normalize(json);
        var arr = result.GetProperty("tracker_list");

        // Assert
        await Assert.That(arr.GetArrayLength()).IsEqualTo(2);
        await Assert.That(arr[0].TryGetProperty("announce_url", out _)).IsTrue();
        await Assert.That(arr[1].TryGetProperty("announce_url", out _)).IsTrue();
    }

    [Test]
    public async Task NormalizeJsonToBytes_PrimitiveValues_ArePreserved()
    {
        // Arrange
        // Values must not be modified — only keys are normalized
        const string json = """{"downloadDir":"/downloads","sizeBytes":1024,"isActive":true}""";

        // Act
        var result = Normalize(json);

        // Assert
        await Assert.That(result.GetProperty("download_dir").GetString()).IsEqualTo("/downloads");
        await Assert.That(result.GetProperty("size_bytes").GetInt64()).IsEqualTo(1024L);
        await Assert.That(result.GetProperty("is_active").GetBoolean()).IsTrue();
    }
}
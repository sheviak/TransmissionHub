using System.Text.Json;
using TransmissionHub.Client.Models;

namespace TransmissionHub.Client.Tests.Internal.Converters;

public class TorrentIdConverterTests
{
    [Test]
    [Arguments(1, "1")]
    [Arguments(123, "123")]
    [Arguments(0, "0")]
    public async Task Write_IntegerId_WritesNumber(int id, string expectedJson)
    {
        // Arrange
        var torrentId = new TorrentId(id);

        // Act
        var json = JsonSerializer.Serialize(torrentId);

        // Assert
        await Assert.That(json).IsEqualTo(expectedJson);
    }

    [Test]
    [Arguments("abcd", "\"abcd\"")]
    [Arguments("recently_active", "\"recently_active\"")]
    public async Task Write_HashString_WritesString(string hash, string expectedJson)
    {
        // Arrange
        var torrentId = new TorrentId(hash);

        // Act
        var json = JsonSerializer.Serialize(torrentId);

        // Assert
        await Assert.That(json).IsEqualTo(expectedJson);
    }

    [Test]
    public async Task Write_EmptyId_WritesNull()
    {
        // Arrange
        var torrentId = default(TorrentId);
        const string expectedJson = "null";

        // Act
        var json = JsonSerializer.Serialize(torrentId);

        // Assert
        await Assert.That(json).IsEqualTo(expectedJson);
    }

    [Test]
    [Arguments("1", 1)]
    [Arguments("123", 123)]
    public async Task Read_NumberToken_ReturnsIntegerId(string json, int expectedId)
    {
        // Arrange & Act
        var result = JsonSerializer.Deserialize<TorrentId>(json);

        // Assert
        await Assert.That(result.Id).IsEqualTo(expectedId);
        await Assert.That(result.HashString).IsNull();
    }

    [Test]
    [Arguments("\"abcd\"", "abcd")]
    [Arguments("\"recently_active\"", "recently_active")]
    public async Task Read_StringToken_ReturnsHashString(string json, string expectedHash)
    {
        // Arrange & Act
        var result = JsonSerializer.Deserialize<TorrentId>(json);

        // Assert
        await Assert.That(result.HashString).IsEqualTo(expectedHash);
        await Assert.That(result.Id).IsNull();
    }

    [Test]
    [Arguments("true")]
    [Arguments("{}")]
    [Arguments("[]")]
    public async Task Read_InvalidToken_ThrowsJsonException(string json)
    {
        // Arrange & Act
        var action = () => JsonSerializer.Deserialize<TorrentId>(json);

        // Assert
        await Assert.That(action).Throws<JsonException>();
    }
}
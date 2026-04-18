using System.Text.Json;
using System.Text.Json.Serialization;
using TransmissionHub.Client.Models;

namespace TransmissionHub.Client.Internal.Converters;

/// <summary>
/// JSON converter for <see cref="TorrentId"/> to support polymorphic serialization (int or string).
/// </summary>
internal class TorrentIdConverter : JsonConverter<TorrentId>
{
    /// <inheritdoc />
    public override TorrentId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => new TorrentId(reader.GetInt32()),
            JsonTokenType.String => new TorrentId(reader.GetString()!),
            _ => throw new JsonException($"Unexpected token type {reader.TokenType} for TorrentId.")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TorrentId value, JsonSerializerOptions options)
    {
        if (value.Id.HasValue)
        {
            writer.WriteNumberValue(value.Id.Value);
        }
        else if (value.HashString is not null)
        {
            writer.WriteStringValue(value.HashString);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
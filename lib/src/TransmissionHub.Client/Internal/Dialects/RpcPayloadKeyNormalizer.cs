using System.Buffers;
using System.Text;
using System.Text.Json;

namespace TransmissionHub.Client.Internal.Dialects;

/// <summary>
/// Normalizes JSON object keys for response deserialization.
/// </summary>
internal static class RpcPayloadKeyNormalizer
{
    /// <summary>
    /// Converts all object keys to snake_case.
    /// </summary>
    public static string NormalizeObjectKeysToSnakeCase(string json)
    {
        using var document = JsonDocument.Parse(json);

        var buffer = new ArrayBufferWriter<byte>();

        using (var writer = new Utf8JsonWriter(buffer))
        {
            WriteNormalized(document.RootElement, writer);
        }

        return Encoding.UTF8.GetString(buffer.WrittenSpan);
    }

    private static void WriteNormalized(JsonElement element, Utf8JsonWriter writer)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
            {
                writer.WriteStartObject();
                foreach (var property in element.EnumerateObject())
                {
                    // Normalize the key:
                    //   1. Replace hyphens with underscores (handles kebab-case: "file-count" → "file_count")
                    //   2. Apply SnakeCaseLower (handles camelCase: "activeTorrentCount" → "active_torrent_count")
                    //      snake_case keys and "total_size" anomaly are idempotent.
                    var normalizedName = property.Name.Contains('-')
                        ? property.Name.Replace('-', '_')
                        : JsonNamingPolicy.SnakeCaseLower.ConvertName(property.Name);

                    writer.WritePropertyName(normalizedName);
                    WriteNormalized(property.Value, writer);
                }

                writer.WriteEndObject();
                return;
            }
            case JsonValueKind.Array:
            {
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                {
                    WriteNormalized(item, writer);
                }

                writer.WriteEndArray();
                return;
            }
            default:
            {
                element.WriteTo(writer);
                return;
            }
        }
    }
}
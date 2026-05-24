using System.Buffers;
using System.Text.Json;

namespace TransmissionHub.Client.Internal.Dialects;

/// <summary>
/// Normalizes JSON object keys for response deserialization.
/// </summary>
internal static class RpcPayloadKeyNormalizer
{
    /// <summary>
    /// Normalizes all object keys in <paramref name="element"/> to <c>snake_case</c>
    /// and returns the result as a UTF-8 byte array ready for <see cref="JsonSerializer"/>.
    /// </summary>
    /// <remarks>
    /// Handles all key formats present in the Transmission legacy protocol:
    /// <list type="bullet">
    ///   <item><c>camelCase</c> — e.g. <c>activeTorrentCount</c> → <c>active_torrent_count</c></item>
    ///   <item><c>kebab-case</c> — e.g. <c>cumulative-stats</c> → <c>cumulative_stats</c></item>
    ///   <item><c>snake_case</c> — already normalized, returned unchanged (idempotent)</item>
    /// </list>
    /// Key conversion uses a stack-allocated <see cref="Span{T}"/> buffer — no heap allocation per key.
    /// </remarks>
    public static ReadOnlySpan<byte> NormalizeJsonToBytes(JsonElement element)
    {
        var buffer = new ArrayBufferWriter<byte>();

        using (var writer = new Utf8JsonWriter(buffer))
        {
            WriteNormalized(element, writer);
        }

        return buffer.WrittenSpan;
    }

    private static void WriteNormalized(JsonElement element, Utf8JsonWriter writer)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
            {
                writer.WriteStartObject();

                // 128 chars covers every Transmission field name with room to spare
                // (longest known key is ~40 chars; worst-case snake expansion doubles it).
                // Allocated once per object level, reused for every property in that object.
                Span<char> nameBuffer = stackalloc char[128];

                foreach (var property in element.EnumerateObject())
                {
                    var written = ToSnakeCase(property.Name, nameBuffer);
                    writer.WritePropertyName(nameBuffer[..written]);
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

    /// <summary>
    /// Converts <paramref name="input"/> to snake_case in-place into <paramref name="output"/>
    /// without any heap allocation.
    /// </summary>
    /// <returns>Number of characters written to <paramref name="output"/>.</returns>
    internal static int ToSnakeCase(ReadOnlySpan<char> input, Span<char> output)
    {
        if (input.IsEmpty)
        {
            return 0;
        }

        var written = 0;

        // Kebab-case fast path: replace '-' with '_', everything else copies as-is.
        if (input.Contains('-'))
        {
            for (var i = 0; i < input.Length; i++)
            {
                output[written++] = input[i] == '-' ? '_' : input[i];
            }

            return written;
        }

        // camelCase / PascalCase → snake_case.
        // snake_case is idempotent: no uppercase letters → no underscores added.
        output[written++] = char.ToLowerInvariant(input[0]);
        for (var i = 1; i < input.Length; i++)
        {
            var c = input[i];
            if (char.IsUpper(c))
            {
                output[written++] = '_';
                output[written++] = char.ToLowerInvariant(c);
            }
            else
            {
                output[written++] = c;
            }
        }

        return written;
    }
}
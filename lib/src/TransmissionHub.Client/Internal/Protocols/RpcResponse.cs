using System.Text.Json;
using System.Text.Json.Serialization;

namespace TransmissionHub.Client.Internal.Protocols;

/// <summary>
/// Represents a Transmission RPC response envelope for protocol versions 16 and 17 (Legacy).
/// </summary>
/// <remarks>
/// Format: <c>{ "result": "success", "arguments": { ... }, "tag": N }</c>
/// </remarks>
internal sealed class RpcResponse
{
    /// <summary>
    /// Gets the result string. Value is <c>"success"</c> on success, or an error message.
    /// </summary>
    [JsonPropertyName("result")]
    public required string Result { get; init; }

    /// <summary>
    /// Gets the response arguments object.
    /// </summary>
    [JsonPropertyName("arguments")]
    public JsonElement? Arguments { get; init; }

    /// <summary>
    /// Gets the optional tag echoed from the request.
    /// </summary>
    [JsonPropertyName("tag")]
    public int? Tag { get; init; }

    /// <summary>
    /// Gets a value indicating whether the RPC call succeeded.
    /// </summary>
    [JsonIgnore]
    public bool IsSuccess => string.Equals(Result, "success", StringComparison.OrdinalIgnoreCase);
}
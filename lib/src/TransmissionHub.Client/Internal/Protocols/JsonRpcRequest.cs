using System.Text.Json.Serialization;

namespace TransmissionHub.Client.Internal.Protocols;

/// <summary>
/// Represents a JSON-RPC 2.0 request envelope for Transmission RPC version 18+.
/// </summary>
/// <remarks>
/// Format: <c>{ "jsonrpc": "2.0", "method": "...", "params": { ... }, "id": N }</c>
/// </remarks>
internal sealed class JsonRpcRequest
{
    /// <summary>
    /// Gets the JSON-RPC protocol version. Always <c>"2.0"</c>.
    /// </summary>
    [JsonPropertyName("jsonrpc")]
    public string JsonRpc { get; } = "2.0";

    /// <summary>
    /// Gets the RPC method name (e.g. <c>torrent_get</c>, <c>session_get</c>).
    /// </summary>
    [JsonPropertyName("method")]
    public required string Method { get; init; }

    /// <summary>
    /// Gets the method parameters object.
    /// </summary>
    [JsonPropertyName("params")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Params { get; init; }

    /// <summary>
    /// Gets the request identifier used to correlate with the response.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Id { get; init; }
}
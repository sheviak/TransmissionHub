using System.Text.Json;
using System.Text.Json.Serialization;

namespace TransmissionHub.Client.Internal.Protocols;

/// <summary>
/// Represents a JSON-RPC 2.0 response envelope for Transmission RPC version 18+.
/// </summary>
/// <remarks>
/// Format: <c>{ "jsonrpc": "2.0", "result": { ... }, "error": { ... }, "id": N }</c>
/// On success <c>result</c> is populated and <c>error</c> is absent.
/// On failure <c>error</c> is populated and <c>result</c> is absent.
/// </remarks>
internal sealed class JsonRpcResponse
{
    /// <summary>
    /// Gets the JSON-RPC protocol version. Always <c>"2.0"</c>.
    /// </summary>
    [JsonPropertyName("jsonrpc")]
    public string JsonRpc { get; init; } = "2.0";

    /// <summary>
    /// Gets the result object, present on success.
    /// </summary>
    [JsonPropertyName("result")]
    public JsonElement? Result { get; init; }

    /// <summary>
    /// Gets the error object, present on failure.
    /// </summary>
    [JsonPropertyName("error")]
    public JsonRpcError? Error { get; init; }

    /// <summary>
    /// Gets the request identifier echoed from the request.
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// Gets a value indicating whether the RPC call succeeded.
    /// </summary>
    [JsonIgnore]
    public bool IsSuccess => Error is null;
}

/// <summary>
/// Represents a JSON-RPC 2.0 error object.
/// </summary>
internal sealed class JsonRpcError
{
    /// <summary>
    /// Gets the numeric error code.
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; init; }

    /// <summary>
    /// Gets the human-readable error message.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; init; }
}
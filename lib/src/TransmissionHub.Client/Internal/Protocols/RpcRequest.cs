namespace TransmissionHub.Client.Internal.Protocols;

/// <summary>
/// Represents a Transmission RPC request envelope for protocol versions 16 and 17 (Legacy).
/// </summary>
/// <remarks>
/// Format: <c>{ "method": "...", "arguments": { ... }, "tag": N }</c>
/// </remarks>
internal sealed class RpcRequest
{
    /// <summary>
    /// Gets the RPC method name (e.g. <c>torrent-get</c>, <c>session-get</c>).
    /// </summary>
    public required string Method { get; init; }

    /// <summary>
    /// Gets the method arguments object.
    /// </summary>
    public object? Arguments { get; init; }

    /// <summary>
    /// Gets an optional tag to correlate request with response.
    /// </summary>
    public int? Tag { get; init; }
}
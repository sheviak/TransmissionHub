using System.Text.Json;
using System.Text.Json.Serialization;

namespace TransmissionHub.Client.Internal.Dialects;

/// <summary>
/// Provides shared <see cref="JsonSerializerOptions"/> instances for RPC dialect serialization.
/// </summary>
internal static class RpcDialectSerializerOptions
{
    /// <summary>
    /// Options configured with <see cref="JsonNamingPolicy.SnakeCaseLower"/>.
    /// Used for deserializing response payloads in both dialects:
    /// <list type="bullet">
    ///   <item><see cref="JsonRpcDialect"/> — directly, since the server already returns snake_case.</item>
    ///   <item><see cref="RpcRequestDialect"/> — after keys have been normalized to snake_case
    ///   via <see cref="RpcPayloadKeyNormalizer"/>.</item>
    /// </list>
    /// </summary>
    public static readonly JsonSerializerOptions SnakeCaseLower = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
    };
}
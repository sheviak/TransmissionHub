using System.Text.Json;
using System.Text.Json.Serialization;
using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Internal.Protocols;

namespace TransmissionHub.Client.Internal.Dialects;

/// <summary>
/// Implements <see cref="IRpcDialect"/> for the Transmission legacy bespoke RPC API (versions 16 and 17).
/// </summary>
/// <remarks>
/// The legacy protocol uses the <see cref="RpcRequest"/> / <see cref="RpcResponse"/> envelope format:
/// <code>
/// Request:  { "method": "torrent-get", "arguments": { ... }, "tag": N }
/// Response: { "result": "success",     "arguments": { ... }, "tag": N }
/// </code>
/// Field names in this dialect are inconsistent: most use <c>camelCase</c>,
/// but some use <c>kebab-case</c> (e.g. <c>file-count</c>, <c>peer-limit</c>).
/// All normalization is handled by <see cref="LegacyFieldNameMapper"/> (requests)
/// and <see cref="RpcPayloadKeyNormalizer"/> (responses → snake_case).
/// </remarks>
internal sealed class RpcRequestDialect : IRpcDialect
{
    /// <summary>
    /// <see cref="JsonSerializerOptions"/> used to serialize the request envelope and arguments using camelCase,
    /// matching the majority of legacy field names.
    /// </summary>
    /// <remarks>
    /// The envelope properties (<c>method</c>, <c>arguments</c>, <c>tag</c>) are protected
    /// by <c>[JsonPropertyName]</c> attributes, so the naming policy only affects the arguments graph.
    /// </remarks>
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <inheritdoc />
    public Result<string> SerializeRequest(RpcMethod method, object? arguments)
    {
        try
        {
            var envelope = new RpcRequest
            {
                Method = ConvertToWireMethodName(method),
                Arguments = arguments,
            };

            var request = JsonSerializer.Serialize(envelope, Options);

            return Result.Ok(request);
        }
        catch (JsonException ex)
        {
            return Result.Fail<string>($"Failed to serialize legacy RPC request: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<string> NormalizeFields(IReadOnlyList<string> pascalCaseFields)
    {
        var result = new string[pascalCaseFields.Count];

        for (var i = 0; i < pascalCaseFields.Count; i++)
        {
            result[i] = LegacyFieldNameMapper.ToLegacyWireName(pascalCaseFields[i]);
        }

        return result;
    }

    /// <inheritdoc />
    public Result<JsonElement> ExtractPayload(string rawJson)
    {
        RpcResponse response;

        try
        {
            response = JsonSerializer.Deserialize<RpcResponse>(rawJson, Options)
                       ?? throw new JsonException("Response deserialized to null.");
        }
        catch (JsonException ex)
        {
            return Result.Fail<JsonElement>($"Failed to parse legacy RPC response: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Fail<JsonElement>($"Unhandled exception while parse JSON-RPC response: {ex.Message}");
        }

        if (!response.IsSuccess)
        {
            return Result.Fail<JsonElement>(response.Result);
        }

        if (response.Arguments is not { } arguments)
        {
            return Result.Ok(JsonSerializer.SerializeToElement(new { }));
        }

        return Result.Ok(arguments);
    }

    /// <inheritdoc />
    public Result<T> Deserialize<T>(JsonElement payload)
    {
        try
        {
            // Normalize all keys (camelCase, kebab-case, underscore anomalies) to snake_case.
            // NormalizeJsonToBytes accepts JsonElement directly — no GetRawText() or reparsing.
            var normalizedBytes = RpcPayloadKeyNormalizer.NormalizeJsonToBytes(payload);

            var result = JsonSerializer.Deserialize<T>(normalizedBytes, RpcDialectSerializerOptions.SnakeCaseLower);

            if (result is null)
            {
                return Result.Fail<T>("Deserialized response payload was null.");
            }

            return Result.Ok(result);
        }
        catch (JsonException ex)
        {
            return Result.Fail<T>($"Failed to deserialize legacy RPC payload: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public string ConvertToWireMethodName(RpcMethod method) => method switch
    {
        RpcMethod.TorrentStart => "torrent-start",
        RpcMethod.TorrentStartNow => "torrent-start-now",
        RpcMethod.TorrentStop => "torrent-stop",
        RpcMethod.TorrentVerify => "torrent-verify",
        RpcMethod.TorrentReannounce => "torrent-reannounce",
        RpcMethod.TorrentSet => "torrent-set",
        RpcMethod.TorrentGet => "torrent-get",
        RpcMethod.TorrentAdd => "torrent-add",
        RpcMethod.TorrentRemove => "torrent-remove",
        RpcMethod.TorrentSetLocation => "torrent-set-location",
        RpcMethod.TorrentRenamePath => "torrent-rename-path",
        RpcMethod.QueueMoveTop => "queue-move-top",
        RpcMethod.QueueMoveUp => "queue-move-up",
        RpcMethod.QueueMoveDown => "queue-move-down",
        RpcMethod.QueueMoveBottom => "queue-move-bottom",
        RpcMethod.SessionGet => "session-get",
        RpcMethod.SessionSet => "session-set",
        RpcMethod.SessionStats => "session-stats",
        RpcMethod.SessionClose => "session-close",
        RpcMethod.FreeSpace => "free-space",
        RpcMethod.PortTest => "port-test",
        RpcMethod.BlocklistUpdate => "blocklist-update",
        RpcMethod.GroupGet => "group-get",
        RpcMethod.GroupSet => "group-set",
        _ => throw new ArgumentOutOfRangeException(nameof(method), method, "Unknown RPC method."),
    };
}
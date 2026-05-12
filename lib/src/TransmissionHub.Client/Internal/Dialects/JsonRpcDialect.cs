using System.Text.Json;
using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Internal.Protocols;

namespace TransmissionHub.Client.Internal.Dialects;

/// <summary>
/// Implements <see cref="IRpcDialect"/> for the Transmission JSON-RPC 2.0 API (version 18+).
/// </summary>
/// <remarks>
/// The modern protocol uses the <see cref="JsonRpcRequest"/> / <see cref="JsonRpcResponse"/> envelope format:
/// <code>
/// Request:  { "jsonrpc": "2.0", "method": "torrent_get", "params": { ... }, "id": N }
/// Response: { "jsonrpc": "2.0", "result": { ... }, "id": N }
/// </code>
/// All field names use <c>snake_case</c> consistently. This dialect uses the built-in
/// <see cref="JsonNamingPolicy.SnakeCaseLower"/> without any custom mapping or normalization.
/// No <see cref="RpcPayloadKeyNormalizer"/> or <see cref="LegacyFieldNameMapper"/> is involved.
/// </remarks>
internal sealed class JsonRpcDialect : IRpcDialect
{
    private static readonly JsonSerializerOptions Options = RpcDialectSerializerOptions.SnakeCaseLower;

    /// <inheritdoc />
    public string SerializeRequest(RpcMethod method, object? arguments)
    {
        var envelope = new JsonRpcRequest
        {
            Method = ToWireMethodName(method),
            Params = arguments,
        };

        return JsonSerializer.Serialize(envelope, Options);
    }

    /// <inheritdoc />
    public IReadOnlyList<string> NormalizeFields(IReadOnlyList<string> pascalCaseFields)
    {
        var result = new string[pascalCaseFields.Count];

        for (var i = 0; i < pascalCaseFields.Count; i++)
        {
            result[i] = JsonNamingPolicy.SnakeCaseLower.ConvertName(pascalCaseFields[i]);
        }

        return result;
    }

    /// <inheritdoc />
    public Result<JsonElement> ExtractPayload(string rawJson)
    {
        JsonRpcResponse response;

        try
        {
            response = JsonSerializer.Deserialize<JsonRpcResponse>(rawJson, Options)
                       ?? throw new JsonException("Response deserialized to null.");
        }
        catch (JsonException ex)
        {
            return Result.Fail<JsonElement>($"Failed to parse JSON-RPC response: {ex.Message}");
        }

        if (!response.IsSuccess)
        {
            var error = response.Error!;
            return Result.Fail<JsonElement>(error.Message, error.Code);
        }

        if (response.Result is not { } result)
        {
            return Result.Ok(JsonSerializer.SerializeToElement(new { }, Options));
        }

        return Result.Ok(result);
    }

    /// <inheritdoc />
    public Result<T> Deserialize<T>(JsonElement payload)
    {
        try
        {
            var result = payload.Deserialize<T>(Options);

            if (result is null)
            {
                return Result.Fail<T>("Deserialized response payload was null.");
            }

            return Result.Ok(result);
        }
        catch (JsonException ex)
        {
            return Result.Fail<T>($"Failed to deserialize JSON-RPC payload: {ex.Message}");
        }
    }

    /// <summary>
    /// Converts a <see cref="RpcMethod"/> to the JSON-RPC 2.0 snake_case wire method name.
    /// </summary>
    private static string ToWireMethodName(RpcMethod method) => method switch
    {
        RpcMethod.TorrentStart => "torrent_start",
        RpcMethod.TorrentStartNow => "torrent_start_now",
        RpcMethod.TorrentStop => "torrent_stop",
        RpcMethod.TorrentVerify => "torrent_verify",
        RpcMethod.TorrentReannounce => "torrent_reannounce",
        RpcMethod.TorrentSet => "torrent_set",
        RpcMethod.TorrentGet => "torrent_get",
        RpcMethod.TorrentAdd => "torrent_add",
        RpcMethod.TorrentRemove => "torrent_remove",
        RpcMethod.TorrentSetLocation => "torrent_set_location",
        RpcMethod.TorrentRenamePath => "torrent_rename_path",
        RpcMethod.QueueMoveTop => "queue_move_top",
        RpcMethod.QueueMoveUp => "queue_move_up",
        RpcMethod.QueueMoveDown => "queue_move_down",
        RpcMethod.QueueMoveBottom => "queue_move_bottom",
        RpcMethod.SessionGet => "session_get",
        RpcMethod.SessionSet => "session_set",
        RpcMethod.SessionStats => "session_stats",
        RpcMethod.SessionClose => "session_close",
        RpcMethod.FreeSpace => "free_space",
        RpcMethod.PortTest => "port_test",
        RpcMethod.BlocklistUpdate => "blocklist_update",
        RpcMethod.GroupGet => "group_get",
        RpcMethod.GroupSet => "group_set",
        _ => throw new ArgumentOutOfRangeException(nameof(method), method, "Unknown RPC method."),
    };
}
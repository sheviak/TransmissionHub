using System.Diagnostics.CodeAnalysis;
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
    /// A <see cref="JsonNamingPolicy"/> that converts property names to the legacy wire format.
    /// </summary>
    /// <remarks>
    /// A thin adapter over <see cref="LegacyFieldNameMapper.ToArgumentName"/>: explicit
    /// kebab-case overrides first, then session overrides, then the camelCase fallback
    /// for the majority of fields. <c>System.Text.Json</c> applies the policy recursively
    /// to every property of the serialized graph, so no per-node rewriting is needed.
    /// All naming decisions stay in <see cref="LegacyFieldNameMapper"/> — the dialect
    /// remains the single transformation point, and the request models shared by all
    /// protocol versions carry no naming attributes.
    /// </remarks>
    private sealed class LegacyNamingPolicy : JsonNamingPolicy
    {
        /// <inheritdoc />
        public override string ConvertName(string name) => LegacyFieldNameMapper.ToArgumentName(name);
    }

    /// <summary>
    /// <see cref="JsonSerializerOptions"/> used to serialize the request envelope and arguments.
    /// </summary>
    /// <remarks>
    /// The <see cref="LegacyNamingPolicy"/> converts argument property keys to the legacy
    /// wire format (e.g. <c>DownloadDir</c> → <c>download-dir</c>). The envelope properties
    /// (<c>method</c>, <c>arguments</c>, <c>tag</c>) and response envelope properties
    /// (<c>result</c>, <c>arguments</c>, <c>tag</c>) are not in the override maps, so they
    /// resolve through the camelCase fallback. The response payload itself is a
    /// <see cref="JsonElement"/> untouched by the policy.
    /// </remarks>
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = new LegacyNamingPolicy(),
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
    [return: NotNullIfNotNull(nameof(pascalCaseFields))]
    public IReadOnlyList<string>? NormalizeFields(RpcMethod method, IReadOnlyList<string>? pascalCaseFields)
    {
        if (pascalCaseFields is not { Count: > 0 })
        {
            return null;
        }

        var result = new string[pascalCaseFields.Count];

        for (var i = 0; i < pascalCaseFields.Count; i++)
        {
            // The same PascalCase name can map to different wire names per method
            // (e.g. DownloadDir is "downloadDir" for torrent-get but "download-dir"
            // for session-get), so the scope must follow the requested method.
            result[i] = method switch
            {
                RpcMethod.TorrentGet => LegacyFieldNameMapper.ToTorrentGetFieldName(pascalCaseFields[i]),
                RpcMethod.SessionGet => LegacyFieldNameMapper.ToSessionFieldName(pascalCaseFields[i]),
                _ => LegacyFieldNameMapper.ToArgumentName(pascalCaseFields[i]),
            };
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
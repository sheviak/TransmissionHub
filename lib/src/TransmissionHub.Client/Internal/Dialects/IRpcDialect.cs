using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using TransmissionHub.Client.Abstractions;

namespace TransmissionHub.Client.Internal.Dialects;

/// <summary>
/// Defines the contract for a Transmission RPC protocol dialect.
/// </summary>
/// <remarks>
/// Two implementations exist:
/// <list type="bullet">
///   <item><see cref="RpcRequestDialect"/> — Legacy bespoke API (RPC versions 16 and 17).</item>
///   <item><see cref="JsonRpcDialect"/> — JSON-RPC 2.0 (RPC version 18+).</item>
/// </list>
/// The dialect encapsulates all version-specific serialization and deserialization logic,
/// keeping the client implementation and domain models free of protocol details.
/// </remarks>
internal interface IRpcDialect
{
    /// <summary>
    /// Serializes a complete RPC request into a ready-to-send JSON string.
    /// </summary>
    /// <remarks>
    /// Internally builds the appropriate envelope (<c>RpcRequest</c> or <c>JsonRpcRequest</c>),
    /// converts the method name to the wire format, and normalizes argument field names.
    /// </remarks>
    /// <param name="method">
    /// The RPC method to invoke. Each dialect converts this to its wire-format string
    /// (e.g. <c>torrent-get</c> for legacy, <c>torrent_get</c> for JSON-RPC 2.0).
    /// </param>
    /// <param name="arguments">
    /// The request arguments object, or <see langword="null"/> for methods with no parameters.
    /// </param>
    /// <returns>
    /// <see cref="Result{T}"/> containing the JSON string ready to be sent as an HTTP request body,
    /// or an <see cref="Error"/> if serialization fails.
    /// </returns>
    public Result<string> SerializeRequest(RpcMethod method, object? arguments);

    /// <summary>
    /// Normalizes a list of field names from PascalCase to the wire format of this dialect.
    /// </summary>
    /// <remarks>
    /// Used for methods that accept a <c>fields</c> parameter, such as <c>torrent-get</c> and <c>session-get</c>.
    /// Returns <see langword="null"/> when <paramref name="pascalCaseFields"/> is <see langword="null"/> or empty,
    /// which signals to the serializer that the <c>fields</c> property should be omitted entirely
    /// (causing Transmission to return all available fields).
    /// </remarks>
    /// <param name="pascalCaseFields">
    /// Field names in PascalCase (e.g. <c>FileCount</c>), or <see langword="null"/> / an empty list to request all fields.
    /// </param>
    /// <returns>
    /// Field names converted to the dialect's wire format,
    /// or <see langword="null"/> if <paramref name="pascalCaseFields"/> was <see langword="null"/> or empty.
    /// </returns>
    [return: NotNullIfNotNull(nameof(pascalCaseFields))]
    public IReadOnlyList<string>? NormalizeFields(IReadOnlyList<string>? pascalCaseFields);

    /// <summary>
    /// Extracts the payload <see cref="JsonElement"/> from a raw JSON response string.
    /// </summary>
    /// <remarks>
    /// For legacy dialect: checks <c>result == "success"</c> and returns the <c>arguments</c> node.
    /// For JSON-RPC dialect: checks <c>error == null</c> and returns the <c>result</c> node.
    /// </remarks>
    /// <param name="rawJson">The raw HTTP response body.</param>
    /// <returns>
    /// <see cref="Result{T}"/> containing the payload element on success,
    /// or a descriptive <see cref="Error"/> on failure.
    /// </returns>
    public Result<JsonElement> ExtractPayload(string rawJson);

    /// <summary>
    /// Deserializes a payload <see cref="JsonElement"/> into a strongly-typed response object.
    /// </summary>
    /// <remarks>
    /// The legacy dialect first normalizes all payload keys to <c>snake_case</c>
    /// via <see cref="RpcPayloadKeyNormalizer"/>, then deserializes using shared
    /// <c>SnakeCaseLower</c> options — the same options used by the modern dialect.
    /// </remarks>
    /// <typeparam name="T">The target response type.</typeparam>
    /// <param name="payload">The payload element extracted by <see cref="ExtractPayload"/>.</param>
    /// <returns>
    /// <see cref="Result{T}"/> containing the deserialized value on success,
    /// or an <see cref="Error"/> if deserialization fails.
    /// </returns>
    public Result<T> Deserialize<T>(JsonElement payload);

    /// <summary>
    /// Converts an <see cref="RpcMethod"/> enum value to its wire-format string representation.
    /// </summary>
    /// <remarks>
    /// Each dialect has its own naming convention for methods.
    /// For example, the legacy dialect uses kebab-case (<c>torrent-get</c>),
    /// while the JSON-RPC 2.0 dialect uses snake_case (<c>torrent_get</c>).
    /// </remarks>
    /// <param name="method">The RPC method to convert.</param>
    /// <returns>The method name as a string, formatted for the specific RPC dialect.</returns>
    public string ConvertToWireMethodName(RpcMethod method);
}
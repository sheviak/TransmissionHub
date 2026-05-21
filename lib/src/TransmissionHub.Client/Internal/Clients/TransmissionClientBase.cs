using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Internal.Dialects;
using TransmissionHub.Client.Models.Requests;
using TransmissionHub.Client.Models.Responses;

namespace TransmissionHub.Client.Internal.Clients;

/// <summary>
/// Implements shared Transmission RPC client behavior.
/// </summary>
internal abstract class TransmissionClientBase : ITransmissionClient
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;
    private readonly IRpcDialect _rpcDialect;
    private readonly TransmissionClientOptions _options;

    /// <summary>
    /// Initializes a new <see cref="TransmissionClientBase"/> instance.
    /// </summary>
    protected TransmissionClientBase(
        HttpClient httpClient,
        IRpcDialect rpcDialect,
        TransmissionClientOptions options,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(rpcDialect);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _rpcDialect = rpcDialect;
        _options = options;
        _logger = logger;
    }

    /// <inheritdoc />
    public virtual Task<Result> TorrentStartAsync(TorrentActionRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.TorrentStart, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> TorrentStartNowAsync(TorrentActionRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.TorrentStartNow, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> TorrentStopAsync(TorrentActionRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.TorrentStop, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> TorrentVerifyAsync(TorrentActionRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.TorrentVerify, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> TorrentReannounceAsync(TorrentActionRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.TorrentReannounce, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> TorrentSetAsync(TorrentSetRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.TorrentSet, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<TorrentGetResponse>> TorrentGetAsync(TorrentGetRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedFields = _rpcDialect.NormalizeFields(request.Fields);
        var updatedRequest = request with { Fields = normalizedFields };
        return ExecuteRawAsync<TorrentGetRequest, TorrentGetResponse>(RpcMethod.TorrentGet, updatedRequest, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<Result<TorrentAddResponse>> TorrentAddAsync(TorrentAddRequest request, CancellationToken cancellationToken = default) =>
        ExecuteRawAsync<TorrentAddRequest, TorrentAddResponse>(RpcMethod.TorrentAdd, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> TorrentRemoveAsync(TorrentRemoveRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.TorrentRemove, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> TorrentSetLocationAsync(TorrentSetLocationRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.TorrentSetLocation, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<TorrentRenamePathResponse>> TorrentRenamePathAsync(TorrentRenamePathRequest request, CancellationToken cancellationToken = default) =>
        ExecuteRawAsync<TorrentRenamePathRequest, TorrentRenamePathResponse>(RpcMethod.TorrentRenamePath, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> QueueMoveTopAsync(QueueMoveRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.QueueMoveTop, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> QueueMoveUpAsync(QueueMoveRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.QueueMoveUp, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> QueueMoveDownAsync(QueueMoveRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.QueueMoveDown, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> QueueMoveBottomAsync(QueueMoveRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.QueueMoveBottom, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<SessionGetResponse>> SessionGetAsync(SessionGetRequest request, CancellationToken cancellationToken = default)
    {
        var fields = request.Fields ?? [];
        var normalizedFields = _rpcDialect.NormalizeFields(fields);
        var updatedRequest = request with { Fields = normalizedFields };
        return ExecuteRawAsync<SessionGetRequest, SessionGetResponse>(RpcMethod.SessionGet, updatedRequest, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<Result> SessionSetAsync(SessionSetRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.SessionSet, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<SessionStatsResponse>> SessionStatsAsync(CancellationToken cancellationToken = default) =>
        ExecuteRawAsync<object, SessionStatsResponse>(RpcMethod.SessionStats, new object(), cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> SessionCloseAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.SessionClose, new object(), cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<FreeSpaceResponse>> FreeSpaceAsync(FreeSpaceRequest request, CancellationToken cancellationToken = default) =>
        ExecuteRawAsync<FreeSpaceRequest, FreeSpaceResponse>(RpcMethod.FreeSpace, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<PortTestResponse>> PortTestAsync(PortTestRequest request, CancellationToken cancellationToken = default) =>
        ExecuteRawAsync<PortTestRequest, PortTestResponse>(RpcMethod.PortTest, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<BlocklistUpdateResponse>> BlocklistUpdateAsync(BlocklistUpdateRequest request, CancellationToken cancellationToken = default) =>
        ExecuteRawAsync<BlocklistUpdateRequest, BlocklistUpdateResponse>(RpcMethod.BlocklistUpdate, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<GroupGetResponse>> GroupGetAsync(GroupGetRequest request, CancellationToken cancellationToken = default) =>
        ExecuteRawAsync<GroupGetRequest, GroupGetResponse>(RpcMethod.GroupGet, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> GroupSetAsync(GroupSetRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.GroupSet, request, cancellationToken);

    private async Task<Result> ExecuteAsync<TRequest>(RpcMethod method, TRequest arguments, CancellationToken cancellationToken)
    {
        var result = await ExecuteRawAsync(method, arguments, cancellationToken);
        return result.IsSuccess ? Result.Ok() : Result.Fail(result.Error!.Value);
    }

    private async Task<Result<TResponse>> ExecuteRawAsync<TRequest, TResponse>(RpcMethod method, TRequest arguments, CancellationToken cancellationToken)
    {
        var result = await ExecuteRawAsync(method, arguments, cancellationToken);

        if (result.IsFailure)
        {
            return Result.Fail<TResponse>(result.Error!.Value);
        }

        var deserializationResult = _rpcDialect.Deserialize<TResponse>(result.Value);

        if (deserializationResult.IsSuccess)
        {
            return deserializationResult;
        }

        _logger.LogSerializationError(deserializationResult.Error!.Value);

        return Result.Fail<TResponse>(deserializationResult.Error!.Value);
    }

    private async Task<Result<JsonElement>> ExecuteRawAsync<TRequest>(RpcMethod method, TRequest arguments, CancellationToken cancellationToken)
    {
        try
        {
            var serializeResult = _rpcDialect.SerializeRequest(method, arguments);

            if (serializeResult.IsFailure)
            {
                _logger.LogSerializationError(serializeResult.Error!.Value);
                return Result.Fail<JsonElement>(serializeResult.Error.Value);
            }

            var jsonRequest = serializeResult.Value;

            if (_options.LogRequests)
            {
                _logger.LogRequest(method, jsonRequest);
            }

            var content = new StringContent(jsonRequest, Encoding.UTF8, MediaTypeNames.Application.Json);

            var httpResponse = await _httpClient.PostAsync(_options.Url, content, cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogHttpError(httpResponse.StatusCode, errorBody);

                return Result.Fail<JsonElement>(new Error($"HTTP error: {(int)httpResponse.StatusCode}", (int)httpResponse.StatusCode));
            }

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            if (_options.LogResponses)
            {
                _logger.LogResponse(method, jsonResponse);
            }

            var payloadResult = _rpcDialect.ExtractPayload(jsonResponse);

            if (payloadResult.IsFailure)
            {
                _logger.LogRpcError(method, payloadResult.Error!.Value.Message);
                return Result.Fail<JsonElement>(payloadResult.Error!.Value);
            }

            return Result.Ok(payloadResult.Value);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogNetworkError(ex);
            return Result.Fail<JsonElement>(new Error($"A network error occurred: {ex.Message}"));
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogOperationCancelled(ex);
            return Result.Fail<JsonElement>(new Error($"The operation was canceled: {ex.Message}"));
        }
        catch (Exception ex)
        {
            _logger.LogUnexpectedError(ex);
            return Result.Fail<JsonElement>(new Error($"An unexpected error occurred: {ex.Message}"));
        }
    }

    protected Result GetUnsupportedMethodResult(RpcMethod method)
    {
        var methodName = _rpcDialect.ConvertToWireMethodName(method);
        _logger.LogUnsupportedMethod(methodName);
        return Result.Fail($"Method '{methodName}' is not supported by this client version.");
    }

    protected Result<T> GetUnsupportedMethodResult<T>(RpcMethod method)
    {
        var methodName = _rpcDialect.ConvertToWireMethodName(method);
        _logger.LogUnsupportedMethod(methodName);
        return Result.Fail<T>($"Method '{methodName}' is not supported by this client version.");
    }
}
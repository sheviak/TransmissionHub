using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Internal.Dialects;
using TransmissionHub.Client.Internal.Validation;
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
    private readonly IValidatorProvider _validatorProvider;

    /// <summary>
    /// Initializes a new <see cref="TransmissionClientBase"/> instance.
    /// </summary>
    protected TransmissionClientBase(
        HttpClient httpClient,
        IRpcDialect rpcDialect,
        TransmissionClientOptions options,
        IValidatorProvider validatorProvider,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(rpcDialect);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(validatorProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _rpcDialect = rpcDialect;
        _options = options;
        _validatorProvider = validatorProvider;
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
        return FetchAsync<TorrentGetRequest, TorrentGetResponse>(RpcMethod.TorrentGet, updatedRequest, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<Result<TorrentAddResponse>> TorrentAddAsync(TorrentAddRequest request, CancellationToken cancellationToken = default) =>
        FetchAsync<TorrentAddRequest, TorrentAddResponse>(RpcMethod.TorrentAdd, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> TorrentRemoveAsync(TorrentRemoveRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.TorrentRemove, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> TorrentSetLocationAsync(TorrentSetLocationRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.TorrentSetLocation, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<TorrentRenamePathResponse>> TorrentRenamePathAsync(TorrentRenamePathRequest request, CancellationToken cancellationToken = default) =>
        FetchAsync<TorrentRenamePathRequest, TorrentRenamePathResponse>(RpcMethod.TorrentRenamePath, request, cancellationToken);

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
        var updatedRequest = new SessionGetRequest { Fields = normalizedFields };
        return FetchAsync<SessionGetRequest, SessionGetResponse>(RpcMethod.SessionGet, updatedRequest, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<Result> SessionSetAsync(SessionSetRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.SessionSet, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<SessionStatsResponse>> SessionStatsAsync(CancellationToken cancellationToken = default) =>
        FetchAsync<SessionStatsResponse>(RpcMethod.SessionStats, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> SessionCloseAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.SessionClose, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<FreeSpaceResponse>> FreeSpaceAsync(FreeSpaceRequest request, CancellationToken cancellationToken = default) =>
        FetchAsync<FreeSpaceRequest, FreeSpaceResponse>(RpcMethod.FreeSpace, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<PortTestResponse>> PortTestAsync(PortTestRequest request, CancellationToken cancellationToken = default) =>
        FetchAsync<PortTestRequest, PortTestResponse>(RpcMethod.PortTest, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<BlocklistUpdateResponse>> BlocklistUpdateAsync(CancellationToken cancellationToken = default) =>
        FetchAsync<BlocklistUpdateResponse>(RpcMethod.BlocklistUpdate, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result<GroupGetResponse>> GroupGetAsync(GroupGetRequest request, CancellationToken cancellationToken = default) =>
        FetchAsync<GroupGetRequest, GroupGetResponse>(RpcMethod.GroupGet, request, cancellationToken);

    /// <inheritdoc />
    public virtual Task<Result> GroupSetAsync(GroupSetRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(RpcMethod.GroupSet, request, cancellationToken);

    /// <summary>
    /// Validates <paramref name="arguments"/>, executes the RPC method, and returns a success/failure result.
    /// </summary>
    private async Task<Result> ExecuteAsync<TRequest>(RpcMethod method, TRequest arguments, CancellationToken cancellationToken) where TRequest : class
    {
        var validationResult = _validatorProvider.Validate(arguments);

        if (validationResult.IsFailure)
        {
            return Result.Fail(validationResult.Error!.Value);
        }

        var result = await ExecuteCoreAsync(method, arguments, cancellationToken);

        return result.IsSuccess ? Result.Ok() : Result.Fail(result.Error!.Value);
    }

    /// <summary>
    /// Executes an RPC method that requires no request body and returns a success/failure result.
    /// </summary>
    private async Task<Result> ExecuteAsync(RpcMethod method, CancellationToken cancellationToken)
    {
        var result = await ExecuteCoreAsync(method, null, cancellationToken);

        return result.IsSuccess ? Result.Ok() : Result.Fail(result.Error!.Value);
    }

    /// <summary>
    /// Executes an RPC method that requires no request body and deserializes the response payload.
    /// </summary>
    private async Task<Result<TResponse>> FetchAsync<TResponse>(RpcMethod method, CancellationToken cancellationToken)
    {
        var result = await ExecuteCoreAsync(method, null, cancellationToken);

        return DeserializePayload<TResponse>(result);
    }

    /// <summary>
    /// Validates <paramref name="arguments"/>, executes the RPC method, and deserializes the response payload.
    /// </summary>
    private async Task<Result<TResponse>> FetchAsync<TRequest, TResponse>(RpcMethod method, TRequest arguments, CancellationToken cancellationToken) where TRequest : class
    {
        var validationResult = _validatorProvider.Validate(arguments);

        if (validationResult.IsFailure)
        {
            return Result.Fail<TResponse>(validationResult.Error!.Value);
        }

        var result = await ExecuteCoreAsync(method, arguments, cancellationToken);

        return DeserializePayload<TResponse>(result);
    }

    /// <summary>
    /// Deserializes a raw <see cref="JsonElement"/> payload into a strongly-typed <typeparamref name="TResponse"/>.
    /// Logs and propagates any deserialization error.
    /// </summary>
    private Result<TResponse> DeserializePayload<TResponse>(Result<JsonElement> result)
    {
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

    /// <summary>
    /// Serializes the request, sends it over HTTP, reads the response, and extracts the payload.
    /// Handles all HTTP errors, RPC-level errors, and exception boundaries.
    /// </summary>
    /// <param name="method">Identifies the Transmission RPC method to invoke.</param>
    /// <param name="arguments">The request arguments, or <see langword="null"/> for methods with no body.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    private async Task<Result<JsonElement>> ExecuteCoreAsync(RpcMethod method, object? arguments, CancellationToken cancellationToken)
    {
        try
        {
            var wireMethodName = _rpcDialect.ConvertToWireMethodName(method);
            var serializeResult = _rpcDialect.SerializeRequest(method, arguments);

            if (serializeResult.IsFailure)
            {
                _logger.LogSerializationError(serializeResult.Error!.Value);
                return Result.Fail<JsonElement>(serializeResult.Error.Value);
            }

            var jsonRequest = serializeResult.Value;

            if (_options.LogRequests)
            {
                _logger.LogRequest(wireMethodName, jsonRequest);
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
                _logger.LogResponse(wireMethodName, jsonResponse);
            }

            var payloadResult = _rpcDialect.ExtractPayload(jsonResponse);

            if (payloadResult.IsFailure)
            {
                _logger.LogRpcError(wireMethodName, payloadResult.Error!.Value.Message);
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
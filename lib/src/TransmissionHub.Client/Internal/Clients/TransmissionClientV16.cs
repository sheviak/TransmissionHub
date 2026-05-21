using Microsoft.Extensions.Logging;
using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Internal.Dialects;
using TransmissionHub.Client.Models.Requests;
using TransmissionHub.Client.Models.Responses;

namespace TransmissionHub.Client.Internal.Clients;

/// <summary>
/// Transmission client for RPC version 16.
/// </summary>
internal sealed class TransmissionClientV16(
    HttpClient httpClient,
    IRpcDialect rpcDialect,
    TransmissionClientOptions options,
    ILogger<TransmissionClientV16> logger)
    : TransmissionClientBase(httpClient, rpcDialect, options, logger)
{
    /// <inheritdoc />
    public override Task<Result<GroupGetResponse>> GroupGetAsync(GroupGetRequest request, CancellationToken cancellationToken = default) =>
        Task.FromResult(GetUnsupportedMethodResult<GroupGetResponse>(RpcMethod.GroupGet));

    /// <inheritdoc />
    public override Task<Result> GroupSetAsync(GroupSetRequest request, CancellationToken cancellationToken = default) =>
        Task.FromResult(GetUnsupportedMethodResult(RpcMethod.GroupSet));
}
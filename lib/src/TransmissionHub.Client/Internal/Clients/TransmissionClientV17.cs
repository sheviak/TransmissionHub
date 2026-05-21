using Microsoft.Extensions.Logging;
using TransmissionHub.Client.Internal.Dialects;

namespace TransmissionHub.Client.Internal.Clients;

/// <summary>
/// Transmission client for RPC version 17.
/// </summary>
internal sealed class TransmissionClientV17(
    HttpClient httpClient,
    IRpcDialect rpcDialect,
    TransmissionClientOptions options,
    ILogger<TransmissionClientV17> logger)
    : TransmissionClientBase(httpClient, rpcDialect, options, logger);
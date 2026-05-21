using Microsoft.Extensions.Logging;
using TransmissionHub.Client.Internal.Dialects;

namespace TransmissionHub.Client.Internal.Clients;

/// <summary>
/// Transmission client for RPC version 18.
/// </summary>
internal sealed class TransmissionClientV18(
    HttpClient httpClient,
    IRpcDialect rpcDialect,
    TransmissionClientOptions options,
    ILogger<TransmissionClientV18> logger)
    : TransmissionClientBase(httpClient, rpcDialect, options, logger);
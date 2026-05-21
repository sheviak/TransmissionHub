using System.Net;
using Microsoft.Extensions.Logging;
using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Internal.Dialects;

namespace TransmissionHub.Client.Internal.Clients;

/// <summary>
/// Provides extension methods for logging in the Transmission client.
/// </summary>
internal static partial class TransmissionClientLoggingExtensions
{
    /// <summary>
    /// Logs an RPC request.
    /// </summary>
    [LoggerMessage(LogLevel.Debug, "RPC Request ({Method}): {JsonRequest}")]
    internal static partial void LogRequest(this ILogger logger, RpcMethod method, string jsonRequest);

    /// <summary>
    /// Logs an RPC response.
    /// </summary>
    [LoggerMessage(LogLevel.Debug, "RPC Response ({Method}): {JsonResponse}")]
    internal static partial void LogResponse(this ILogger logger, RpcMethod method, string jsonResponse);

    /// <summary>
    /// Logs an HTTP request failure.
    /// </summary>
    [LoggerMessage(LogLevel.Error, "HTTP request failed with status code {StatusCode}. Response: {Response}")]
    internal static partial void LogHttpError(this ILogger logger, HttpStatusCode statusCode, string response);

    /// <summary>
    /// Logs an RPC call failure.
    /// </summary>
    [LoggerMessage(LogLevel.Error, "RPC call {Method} failed: {RpcResult}")]
    internal static partial void LogRpcError(this ILogger logger, RpcMethod method, string rpcResult);

    /// <summary>
    /// Logs a serialization error.
    /// </summary>
    [LoggerMessage(LogLevel.Error, "A serialization error occurred. {Error}")]
    internal static partial void LogSerializationError(this ILogger logger, Error error);

    /// <summary>
    /// Logs a network error.
    /// </summary>
    [LoggerMessage(LogLevel.Error, "A network error occurred.")]
    internal static partial void LogNetworkError(this ILogger logger, Exception ex);

    /// <summary>
    /// Logs an operation cancellation.
    /// </summary>
    [LoggerMessage(LogLevel.Warning, "The operation was canceled.")]
    internal static partial void LogOperationCancelled(this ILogger logger, Exception ex);

    /// <summary>
    /// Logs an unexpected error.
    /// </summary>
    [LoggerMessage(LogLevel.Critical, "An unexpected error occurred.")]
    internal static partial void LogUnexpectedError(this ILogger logger, Exception ex);

    /// <summary>
    /// Logs an unsupported RPC method call.
    /// </summary>
    [LoggerMessage(LogLevel.Error, "Method {MethodName} is not supported by this RPC version.")]
    internal static partial void LogUnsupportedMethod(this ILogger logger, string methodName);
}
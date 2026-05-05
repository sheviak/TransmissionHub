namespace TransmissionHub.Client.Abstractions;

/// <summary>
/// Represents an error returned by the Transmission RPC API or the HTTP transport layer.
/// </summary>
public readonly struct Error
{
    /// <summary>
    /// Gets the human-readable error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets an optional error code.
    /// </summary>
    /// <remarks>
    /// For RPC errors this may contain the JSON-RPC error code (v18+).
    /// For transport errors it may contain the HTTP status code.
    /// </remarks>
    public int? Code { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> struct.
    /// </summary>
    /// <param name="message">The human-readable error message.</param>
    /// <param name="code">An optional error code.</param>
    public Error(string message, int? code = null)
    {
        Message = message;
        Code = code;
    }

    /// <inheritdoc />
    public override string ToString() => Code.HasValue
        ? $"[{Code}] {Message}"
        : Message;
}
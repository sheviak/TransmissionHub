namespace TransmissionHub.Client.Models.Responses;

/// <summary>
/// Response to the port test request.
/// </summary>
public record PortTestResponse
{
    /// <summary>
    /// True if the port is accessible.
    /// </summary>
    public bool PortIsOpen { get; init; }

    /// <summary>
    /// 'ipv4' or 'ipv6'. Unset if it cannot be determined.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18.
    /// </remarks>
    public string? IpProtocol { get; init; }
}
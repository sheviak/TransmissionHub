namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// Request to test if the incoming peer port is accessible from the outside world.
/// </summary>
public record PortTestRequest
{
    /// <summary>
    /// The IP protocol to use for the port test.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18.
    /// </remarks>
    public string? IpProtocol { get; init; }
}
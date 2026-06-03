namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// Request to configure a bandwidth group.
/// </summary>
/// <remarks>
/// Added in RPC version 17.
/// </remarks>
public record GroupSetRequest
{
    /// <summary>
    /// True if session upload limits are honored.
    /// </summary>
    public required bool HonorsSessionLimits { get; init; }

    /// <summary>
    /// Bandwidth group name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Max global download speed (KBps).
    /// </summary>
    public required int SpeedLimitDown { get; init; }

    /// <summary>
    /// True means enabled.
    /// </summary>
    public required bool SpeedLimitDownEnabled { get; init; }

    /// <summary>
    /// Max global upload speed (KBps).
    /// </summary>
    public required int SpeedLimitUp { get; init; }

    /// <summary>
    /// True means enabled.
    /// </summary>
    public required bool SpeedLimitUpEnabled { get; init; }
}
namespace TransmissionHub.Client.Models.Responses;

/// <summary>
/// Response containing bandwidth groups.
/// </summary>
/// <remarks>
/// Added in RPC version 17.
/// </remarks>
public record GroupGetResponse
{
    /// <summary>
    /// A list of bandwidth group description objects.
    /// </summary>
    public IReadOnlyList<BandwidthGroup>? Group { get; init; }
}

/// <summary>
/// Describes a bandwidth group.
/// </summary>
public record BandwidthGroup
{
    /// <summary>
    /// True if session upload limits are honored.
    /// </summary>
    public bool? HonorsSessionLimits { get; init; }

    /// <summary>
    /// Bandwidth group name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Max global download speed (KBps).
    /// </summary>
    public int? SpeedLimitDown { get; init; }

    /// <summary>
    /// True means enabled.
    /// </summary>
    public bool? SpeedLimitDownEnabled { get; init; }

    /// <summary>
    /// Max global upload speed (KBps).
    /// </summary>
    public int? SpeedLimitUp { get; init; }

    /// <summary>
    /// True means enabled.
    /// </summary>
    public bool? SpeedLimitUpEnabled { get; init; }
}
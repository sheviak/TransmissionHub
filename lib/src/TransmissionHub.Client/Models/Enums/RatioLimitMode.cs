namespace TransmissionHub.Client.Models.Enums;

/// <summary>
/// Mode for seeding ratio limit.
/// </summary>
/// <remarks>
/// Corresponds to 'tr_ratiolimit' in libtransmission.
/// </remarks>
public enum RatioLimitMode
{
    /// <summary>
    /// Follow the global session limit.
    /// </summary>
    Global = 0,

    /// <summary>
    /// Use the torrent-specific limit.
    /// </summary>
    Single = 1,

    /// <summary>
    /// Unlimited seeding (no limit).
    /// </summary>
    Unlimited = 2,
}
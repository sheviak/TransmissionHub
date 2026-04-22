namespace TransmissionHub.Client.Models.Enums;

/// <summary>
/// Mode for idle limit seeding.
/// </summary>
/// <remarks>
/// Corresponds to 'tr_idlelimit' in libtransmission.
/// </remarks>
public enum IdleLimitMode
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
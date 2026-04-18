namespace TransmissionHub.Client.Models.Enums;

/// <summary>
/// Bandwidth priority mode for torrents and files.
/// </summary>
/// <remarks>
/// Corresponds to 'tr_priority_t' in libtransmission.
/// </remarks>
public enum PriorityMode
{
    /// <summary>
    /// Low priority.
    /// </summary>
    Low = -1,

    /// <summary>
    /// Normal priority.
    /// </summary>
    Normal = 0,

    /// <summary>
    /// High priority.
    /// </summary>
    High = 1,
}
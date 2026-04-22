namespace TransmissionHub.Client.Models.Enums;

/// <summary>
/// Status of a torrent.
/// </summary>
/// <remarks>
/// Corresponds to 'tr_status' in libtransmission.
/// </remarks>
public enum TorrentStatus
{
    /// <summary>
    /// Torrent is stopped.
    /// </summary>
    Stopped = 0,

    /// <summary>
    /// Torrent is queued to verify local data.
    /// </summary>
    VerifyWait = 1,

    /// <summary>
    /// Torrent is verifying local data.
    /// </summary>
    Verify = 2,

    /// <summary>
    /// Torrent is queued to download.
    /// </summary>
    DownloadWait = 3,

    /// <summary>
    /// Torrent is downloading.
    /// </summary>
    Download = 4,

    /// <summary>
    /// Torrent is queued to seed.
    /// </summary>
    SeedWait = 5,

    /// <summary>
    /// Torrent is seeding.
    /// </summary>
    Seed = 6,
}
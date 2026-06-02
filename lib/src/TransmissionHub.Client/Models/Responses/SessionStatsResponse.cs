namespace TransmissionHub.Client.Models.Responses;

/// <summary>
/// Status of the torrent session.
/// </summary>
public record SessionStatsResponse
{
    /// <summary>
    /// Number of active torrents.
    /// </summary>
    public int ActiveTorrentCount { get; init; }

    /// <summary>
    /// Current download speed.
    /// </summary>
    public int DownloadSpeed { get; init; }

    /// <summary>
    /// Number of paused torrents.
    /// </summary>
    public int PausedTorrentCount { get; init; }

    /// <summary>
    /// Total number of torrents.
    /// </summary>
    public int TorrentCount { get; init; }

    /// <summary>
    /// Current upload speed.
    /// </summary>
    public int UploadSpeed { get; init; }

    /// <summary>
    /// Cumulative session statistics.
    /// </summary>
    public SessionStats CumulativeStats { get; init; } = null!;

    /// <summary>
    /// Current session statistics.
    /// </summary>
    public SessionStats CurrentStats { get; init; } = null!;
}

/// <summary>
/// Statistics for the current or cumulative session.
/// </summary>
public record SessionStats
{
    /// <summary>
    /// Total uploaded bytes.
    /// </summary>
    public long UploadedBytes { get; init; }

    /// <summary>
    /// Total downloaded bytes.
    /// </summary>
    public long DownloadedBytes { get; init; }

    /// <summary>
    /// Number of files added.
    /// </summary>
    public int FilesAdded { get; init; }

    /// <summary>
    /// Number of seconds active.
    /// </summary>
    public int SecondsActive { get; init; }

    /// <summary>
    /// Session counts.
    /// </summary>
    public int SessionCount { get; init; }
}
using TransmissionHub.Client.Models.Enums;

namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// Request to add a torrent.
/// </summary>
public record TorrentAddRequest
{
    /// <summary>
    /// Pointer to a string of one or more cookies.
    /// </summary>
    public string? Cookies { get; init; }

    /// <summary>
    /// Path to download the torrent to.
    /// </summary>
    public string? DownloadDir { get; init; }

    /// <summary>
    /// Filename or URL of the .torrent file.
    /// </summary>
    public string? Filename { get; init; }

    /// <summary>
    /// Array of string labels.
    /// </summary>
    public IReadOnlyList<string>? Labels { get; init; }

    /// <summary>
    /// Base64-encoded .torrent content.
    /// </summary>
    public string? Metainfo { get; init; }

    /// <summary>
    /// If true, don't start the torrent.
    /// </summary>
    public bool? Paused { get; init; }

    /// <summary>
    /// Maximum number of peers.
    /// </summary>
    public int? PeerLimit { get; init; }

    /// <summary>
    /// Torrent's bandwidth priority.
    /// </summary>
    public PriorityMode? BandwidthPriority { get; init; }

    /// <summary>
    /// Indices of file(s) to download.
    /// </summary>
    public IReadOnlyList<int>? FilesWanted { get; init; }

    /// <summary>
    /// Indices of file(s) to not download.
    /// </summary>
    public IReadOnlyList<int>? FilesUnwanted { get; init; }

    /// <summary>
    /// Indices of high-priority file(s).
    /// </summary>
    public IReadOnlyList<int>? PriorityHigh { get; init; }

    /// <summary>
    /// Indices of low-priority file(s).
    /// </summary>
    public IReadOnlyList<int>? PriorityLow { get; init; }

    /// <summary>
    /// Indices of normal-priority file(s).
    /// </summary>
    public IReadOnlyList<int>? PriorityNormal { get; init; }

    /// <summary>
    /// Download torrent pieces sequentially.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18.
    /// </remarks>
    public bool? SequentialDownload { get; init; }

    /// <summary>
    /// Download from a specific piece when sequential download is enabled.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18.
    /// </remarks>
    public int? SequentialDownloadFromPiece { get; init; }
}
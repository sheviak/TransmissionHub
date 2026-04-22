using TransmissionHub.Client.Models.Enums;

namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// Mutator request for torrent properties.
/// </summary>
public record TorrentSetRequest
{
    /// <summary>
    /// This torrent's bandwidth priority.
    /// </summary>
    public PriorityMode? BandwidthPriority { get; init; }

    /// <summary>
    /// Maximum download speed (KBps).
    /// </summary>
    public int? DownloadLimit { get; init; }

    /// <summary>
    /// True if downloadLimit is honored.
    /// </summary>
    public bool? DownloadLimited { get; init; }

    /// <summary>
    /// Indices of file(s) to not download.
    /// </summary>
    public IReadOnlyList<int>? FilesUnwanted { get; init; }

    /// <summary>
    /// Indices of file(s) to download.
    /// </summary>
    public IReadOnlyList<int>? FilesWanted { get; init; }

    /// <summary>
    /// The name of this torrent's bandwidth group.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public string? Group { get; init; }

    /// <summary>
    /// True if session upload limits are honored.
    /// </summary>
    public bool? HonorsSessionLimits { get; init; }

    /// <summary>
    /// Torrent list, as described in <see cref="TorrentId"/>.
    /// </summary>
    public IReadOnlyList<TorrentId>? Ids { get; init; }

    /// <summary>
    /// Array of string labels.
    /// </summary>
    public IReadOnlyList<string>? Labels { get; init; }

    /// <summary>
    /// New location of the torrent's content.
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    /// Maximum number of peers.
    /// </summary>
    public int? PeerLimit { get; init; }

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
    /// Position of this torrent in its queue.
    /// </summary>
    public int? QueuePosition { get; init; }

    /// <summary>
    /// Torrent-level number of minutes of seeding inactivity.
    /// </summary>
    public int? SeedIdleLimit { get; init; }

    /// <summary>
    /// Which seeding inactivity to use.
    /// </summary>
    public IdleLimitMode? SeedIdleMode { get; init; }

    /// <summary>
    /// Torrent-level seeding ratio.
    /// </summary>
    public double? SeedRatioLimit { get; init; }

    /// <summary>
    /// Which ratio to use.
    /// </summary>
    public RatioLimitMode? SeedRatioMode { get; init; }

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

    /// <summary>
    /// Add tracker URLs.
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC version 17. Use TrackerList instead.
    /// </remarks>
    public IReadOnlyList<string>? TrackerAdd { get; init; }

    /// <summary>
    /// String of announce URLs, one per line, and a blank line between tiers.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public string? TrackerList { get; init; }

    /// <summary>
    /// Remove tracker by index.
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC version 17. Use TrackerList instead.
    /// </remarks>
    public IReadOnlyList<int>? TrackerRemove { get; init; }

    /// <summary>
    /// Replace tracker by index.
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC version 17. Use TrackerList instead.
    /// </remarks>
    public IReadOnlyList<string>? TrackerReplace { get; init; }

    /// <summary>
    /// Maximum upload speed (KBps).
    /// </summary>
    public int? UploadLimit { get; init; }

    /// <summary>
    /// True if uploadLimit is honored.
    /// </summary>
    public bool? UploadLimited { get; init; }
}
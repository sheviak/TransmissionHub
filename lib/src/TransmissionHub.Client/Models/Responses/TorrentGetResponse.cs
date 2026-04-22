using TransmissionHub.Client.Models.Enums;

namespace TransmissionHub.Client.Models.Responses;

/// <summary>
/// Response containing torrent properties.
/// </summary>
public record TorrentGetResponse
{
    /// <summary>
    /// Array of torrent objects containing requested properties.
    /// </summary>
    public IReadOnlyList<TorrentInfo>? Torrents { get; init; }

    /// <summary>
    /// Array of torrent-id numbers of recently-removed torrents (if 'recently-active' was requested).
    /// </summary>
    public IReadOnlyList<int>? Removed { get; init; }
}

/// <summary>
/// Detailed information about a single torrent.
/// </summary>
public record TorrentInfo
{
    /// <summary>
    /// The last time we saw any activity on this torrent (unixtime).
    /// </summary>
    public int? ActivityDate { get; init; }

    /// <summary>
    /// The time this torrent was added (unixtime).
    /// </summary>
    public int? AddedDate { get; init; }

    /// <summary>
    /// Represents the number of connected peers that have each piece.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public IReadOnlyList<int>? Availability { get; init; }

    /// <summary>
    /// This torrent's bandwidth priority.
    /// </summary>
    public PriorityMode? BandwidthPriority { get; init; }

    /// <summary>
    /// Number of bytes completed for each file.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18.
    /// </remarks>
    public IReadOnlyList<long>? BytesCompleted { get; init; }

    /// <summary>
    /// The torrent's comment.
    /// </summary>
    public string? Comment { get; init; }

    /// <summary>
    /// Number of bytes we've downloaded that were corrupt.
    /// </summary>
    public int? CorruptEver { get; init; }

    /// <summary>
    /// The torrent's creator.
    /// </summary>
    public string? Creator { get; init; }

    /// <summary>
    /// The time this torrent was created (unixtime).
    /// </summary>
    public int? DateCreated { get; init; }

    /// <summary>
    /// Number of bytes we want that we don't have yet.
    /// </summary>
    public long? DesiredAvailable { get; init; }

    /// <summary>
    /// The time this torrent finished downloading (unixtime).
    /// </summary>
    public int? DoneDate { get; init; }

    /// <summary>
    /// The directory the torrent's content is stored in.
    /// </summary>
    public string? DownloadDir { get; init; }

    /// <summary>
    /// Cumulative number of bytes downloaded for this torrent.
    /// </summary>
    public long? DownloadedEver { get; init; }

    /// <summary>
    /// Maximum download speed (KBps).
    /// </summary>
    public int? DownloadLimit { get; init; }

    /// <summary>
    /// True if downloadLimit is honored.
    /// </summary>
    public bool? DownloadLimited { get; init; }

    /// <summary>
    /// The last time we edited this torrent's metadata (unixtime).
    /// </summary>
    public int? EditDate { get; init; }

    /// <summary>
    /// Error code.
    /// </summary>
    public int? Error { get; init; }

    /// <summary>
    /// Description of the error.
    /// </summary>
    public string? ErrorString { get; init; }

    /// <summary>
    /// Estimated number of seconds until download is done.
    /// </summary>
    public int? Eta { get; init; }

    /// <summary>
    /// Estimated number of seconds until seeding ratio is reached.
    /// </summary>
    public int? EtaIdle { get; init; }

    /// <summary>
    /// Number of files in the torrent.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public int? FileCount { get; init; }

    /// <summary>
    /// Array of file objects.
    /// </summary>
    public IReadOnlyList<TorrentFile>? Files { get; init; }

    /// <summary>
    /// Array of file stats objects.
    /// </summary>
    public IReadOnlyList<FileStat>? FileStats { get; init; }

    /// <summary>
    /// The name of this torrent's bandwidth group.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public string? Group { get; init; }

    /// <summary>
    /// The torrent's info hash.
    /// </summary>
    public string? HashString { get; init; }

    /// <summary>
    /// Number of bytes we have that haven't been verified yet.
    /// </summary>
    public long? HaveUnchecked { get; init; }

    /// <summary>
    /// Number of bytes we have that have been verified.
    /// </summary>
    public long? HaveValid { get; init; }

    /// <summary>
    /// True if session upload limits are honored.
    /// </summary>
    public bool? HonorsSessionLimits { get; init; }

    /// <summary>
    /// The torrent's numeric ID.
    /// </summary>
    public int? Id { get; init; }

    /// <summary>
    /// True if the torrent is finished.
    /// </summary>
    public bool? IsFinished { get; init; }

    /// <summary>
    /// True if the torrent is private.
    /// </summary>
    public bool? IsPrivate { get; init; }

    /// <summary>
    /// True if the torrent is stalled.
    /// </summary>
    public bool? IsStalled { get; init; }

    /// <summary>
    /// Array of string labels.
    /// </summary>
    public IReadOnlyList<string>? Labels { get; init; }

    /// <summary>
    /// Number of bytes until the download is successful.
    /// </summary>
    public long? LeftUntilDone { get; init; }

    /// <summary>
    /// The torrent's magnet link.
    /// </summary>
    public string? MagnetLink { get; init; }

    /// <summary>
    /// The next time an announce will be sent (unixtime).
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC version 18. Never worked.
    /// </remarks>
    public int? ManualAnnounceTime { get; init; }

    /// <summary>
    /// Maximum number of connected peers.
    /// </summary>
    public int? MaxConnectedPeers { get; init; }

    /// <summary>
    /// Fraction of metadata we have [0...1].
    /// </summary>
    public double? MetadataPercentComplete { get; init; }

    /// <summary>
    /// Torrent name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Maximum number of peers.
    /// </summary>
    public int? PeerLimit { get; init; }

    /// <summary>
    /// Array of peer objects.
    /// </summary>
    public IReadOnlyList<PeerInfo>? Peers { get; init; }

    /// <summary>
    /// Number of peers we're currently connected to.
    /// </summary>
    public int? PeersConnected { get; init; }

    /// <summary>
    /// Statistics about where our peers came from.
    /// </summary>
    public PeersFrom? PeersFrom { get; init; }

    /// <summary>
    /// Number of peers we're uploading to.
    /// </summary>
    public int? PeersGettingFromUs { get; init; }

    /// <summary>
    /// Number of peers we're downloading from.
    /// </summary>
    public int? PeersSendingToUs { get; init; }

    /// <summary>
    /// Fraction of the torrent we have [0...1].
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public double? PercentComplete { get; init; }

    /// <summary>
    /// Fraction of the wanted files we have [0...1].
    /// </summary>
    public double? PercentDone { get; init; }

    /// <summary>
    /// Base64-encoded string of piece flags.
    /// </summary>
    public string? Pieces { get; init; }

    /// <summary>
    /// Number of pieces in the torrent.
    /// </summary>
    public int? PieceCount { get; init; }

    /// <summary>
    /// Number of bytes in each piece.
    /// </summary>
    public int? PieceSize { get; init; }

    /// <summary>
    /// Array of tr_priority_t for each file.
    /// </summary>
    public IReadOnlyList<PriorityMode>? Priorities { get; init; }

    /// <summary>
    /// The primary MIME type of the torrent content.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public string? PrimaryMimeType { get; init; }

    /// <summary>
    /// Position of this torrent in its queue.
    /// </summary>
    public int? QueuePosition { get; init; }

    /// <summary>
    /// Torrent's download speed (B/s).
    /// </summary>
    public int? RateDownload { get; init; }

    /// <summary>
    /// Torrent's upload speed (B/s).
    /// </summary>
    public int? RateUpload { get; init; }

    /// <summary>
    /// Fraction of the torrent we've verified [0...1].
    /// </summary>
    public double? RecheckProgress { get; init; }

    /// <summary>
    /// Cumulative number of seconds we've been downloading this torrent.
    /// </summary>
    public int? SecondsDownloading { get; init; }

    /// <summary>
    /// Cumulative number of seconds we've been seeding this torrent.
    /// </summary>
    public int? SecondsSeeding { get; init; }

    /// <summary>
    /// Number of minutes of seeding inactivity before the torrent is stopped.
    /// </summary>
    public int? SeedIdleLimit { get; init; }

    /// <summary>
    /// Which seeding inactivity limit to use.
    /// </summary>
    public IdleLimitMode? SeedIdleMode { get; init; }

    /// <summary>
    /// Seeding ratio before the torrent is stopped.
    /// </summary>
    public double? SeedRatioLimit { get; init; }

    /// <summary>
    /// Which seeding ratio limit to use.
    /// </summary>
    public RatioLimitMode? SeedRatioMode { get; init; }

    /// <summary>
    /// Download pieces sequentially.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18.
    /// </remarks>
    public bool? SequentialDownload { get; init; }

    /// <summary>
    /// Start sequential download from this piece.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18.
    /// </remarks>
    public int? SequentialDownloadFromPiece { get; init; }

    /// <summary>
    /// Total bytes we will have when download is done.
    /// </summary>
    public long? SizeWhenDone { get; init; }

    /// <summary>
    /// The time this torrent was started (unixtime).
    /// </summary>
    public int? StartDate { get; init; }

    /// <summary>
    /// Current status of the torrent.
    /// </summary>
    public TorrentStatus? Status { get; init; }

    /// <summary>
    /// Path to the .torrent file on the server.
    /// </summary>
    public string? TorrentFile { get; init; }

    /// <summary>
    /// Total bytes in the torrent.
    /// </summary>
    public long? TotalSize { get; init; }

    /// <summary>
    /// Array of tracker objects.
    /// </summary>
    public IReadOnlyList<Tracker>? Trackers { get; init; }

    /// <summary>
    /// String of announce URLs, one per line.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public string? TrackerList { get; init; }

    /// <summary>
    /// Array of tracker stats objects.
    /// </summary>
    public IReadOnlyList<TrackerStats>? TrackerStats { get; init; }

    /// <summary>
    /// Cumulative number of bytes uploaded for this torrent.
    /// </summary>
    public long? UploadedEver { get; init; }

    /// <summary>
    /// Maximum upload speed (KBps).
    /// </summary>
    public int? UploadLimit { get; init; }

    /// <summary>
    /// True if uploadLimit is honored.
    /// </summary>
    public bool? UploadLimited { get; init; }

    /// <summary>
    /// Seed ratio reached for this torrent.
    /// </summary>
    public double? UploadRatio { get; init; }

    /// <summary>
    /// Array of booleans indicating which files we want to download.
    /// </summary>
    public IReadOnlyList<bool>? Wanted { get; init; }

    /// <summary>
    /// Array of webseed URL strings.
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC 4.2.0+. Use WebseedsEx instead.
    /// </remarks>
    public IReadOnlyList<string>? Webseeds { get; init; }

    /// <summary>
    /// Array of webseed objects.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 19/Transmission 4.2.0.
    /// </remarks>
    public IReadOnlyList<WebseedEx>? WebseedsEx { get; init; }

    /// <summary>
    /// Number of webseeds we're currently downloading from.
    /// </summary>
    public int? WebseedsSendingToUs { get; init; }
}

/// <summary>
/// Information about a single file in a torrent.
/// </summary>
public record TorrentFile
{
    /// <summary>
    /// Number of bytes completed for this file.
    /// </summary>
    public long? BytesCompleted { get; init; }

    /// <summary>
    /// Total length of the file in bytes.
    /// </summary>
    public long? Length { get; init; }

    /// <summary>
    /// File name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Index of the first piece containing data for this file.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18.
    /// </remarks>
    public int? BeginPiece { get; init; }

    /// <summary>
    /// Index of the last piece containing data for this file.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18.
    /// </remarks>
    public int? EndPiece { get; init; }
}

/// <summary>
/// Statistics for a single file.
/// </summary>
public record FileStat
{
    /// <summary>
    /// Number of bytes completed for this file.
    /// </summary>
    public long? BytesCompleted { get; init; }

    /// <summary>
    /// True if we want to download this file.
    /// </summary>
    public bool? Wanted { get; init; }

    /// <summary>
    /// File priority.
    /// </summary>
    public PriorityMode? Priority { get; init; }
}

/// <summary>
/// Information about a connected peer.
/// </summary>
public record PeerInfo
{
    /// <summary>
    /// Peer's IP address.
    /// </summary>
    public string? Address { get; init; }

    /// <summary>
    /// Total bytes sent to this peer.
    /// </summary>
    public long? BytesToClient { get; init; }

    /// <summary>
    /// Total bytes received from this peer.
    /// </summary>
    public long? BytesToPeer { get; init; }

    /// <summary>
    /// True if the client is choked by the peer.
    /// </summary>
    public bool? ClientIsChoked { get; init; }

    /// <summary>
    /// True if the client is interested in the peer.
    /// </summary>
    public bool? ClientIsInterested { get; init; }

    /// <summary>
    /// Peer's client name.
    /// </summary>
    public string? ClientName { get; init; }

    /// <summary>
    /// Flags string describing the peer connection.
    /// </summary>
    public string? FlagStr { get; init; }

    /// <summary>
    /// True if we're currently downloading from this peer.
    /// </summary>
    public bool? IsDownloadingFrom { get; init; }

    /// <summary>
    /// True if the connection is encrypted.
    /// </summary>
    public bool? IsEncrypted { get; init; }

    /// <summary>
    /// True if the connection is an incoming one.
    /// </summary>
    public bool? IsIncoming { get; init; }

    /// <summary>
    /// True if we're currently uploading to this peer.
    /// </summary>
    public bool? IsUploadingTo { get; init; }

    /// <summary>
    /// True if the connection is using uTP.
    /// </summary>
    public bool? IsUtp { get; init; }

    /// <summary>
    /// Peer's ID string.
    /// </summary>
    public string? PeerId { get; init; }

    /// <summary>
    /// True if the peer is choked by the client.
    /// </summary>
    public bool? PeerIsChoked { get; init; }

    /// <summary>
    /// True if the peer is interested in the client.
    /// </summary>
    public bool? PeerIsInterested { get; init; }

    /// <summary>
    /// Peer's port.
    /// </summary>
    public int? Port { get; init; }

    /// <summary>
    /// Fraction of the torrent the peer has [0...1].
    /// </summary>
    public double? Progress { get; init; }

    /// <summary>
    /// Download speed from this peer (B/s).
    /// </summary>
    public int? RateToClient { get; init; }

    /// <summary>
    /// Upload speed to this peer (B/s).
    /// </summary>
    public int? RateToPeer { get; init; }
}

/// <summary>
/// Statistics about where our peers were discovered.
/// </summary>
public record PeersFrom
{
    /// <summary>
    /// Number of peers from the cache.
    /// </summary>
    public int? FromCache { get; init; }

    /// <summary>
    /// Number of peers from DHT.
    /// </summary>
    public int? FromDht { get; init; }

    /// <summary>
    /// Number of peers from incoming connections.
    /// </summary>
    public int? FromIncoming { get; init; }

    /// <summary>
    /// Number of peers from LPD.
    /// </summary>
    public int? FromLpd { get; init; }

    /// <summary>
    /// Number of peers from LTEP.
    /// </summary>
    public int? FromLtep { get; init; }

    /// <summary>
    /// Number of peers from PEX.
    /// </summary>
    public int? FromPex { get; init; }

    /// <summary>
    /// Number of peers from trackers.
    /// </summary>
    public int? FromTracker { get; init; }
}

/// <summary>
/// Information about a tracker.
/// </summary>
public record Tracker
{
    /// <summary>
    /// The tracker's announce URL.
    /// </summary>
    public string? Announce { get; init; }

    /// <summary>
    /// The tracker's ID.
    /// </summary>
    public int? Id { get; init; }

    /// <summary>
    /// The tracker's scrape URL.
    /// </summary>
    public string? Scrape { get; init; }

    /// <summary>
    /// The host name of the tracker.
    /// </summary>
    public string? Sitename { get; init; }

    /// <summary>
    /// The tracker's tier.
    /// </summary>
    public int? Tier { get; init; }
}

/// <summary>
/// Detailed statistics for a tracker.
/// </summary>
public record TrackerStats
{
    /// <summary>
    /// The tracker's announce URL.
    /// </summary>
    public string? Announce { get; init; }

    /// <summary>
    /// The current state of announcing to this tracker.
    /// </summary>
    public int? AnnounceState { get; init; }

    /// <summary>
    /// Cumulative number of successful downloads reported by the tracker.
    /// </summary>
    public int? DownloadCount { get; init; }

    /// <summary>
    /// Number of downloaders currently reported by the tracker.
    /// </summary>
    public int? DownloaderCount { get; init; }

    /// <summary>
    /// True if we have ever announced to this tracker.
    /// </summary>
    public bool? HasAnnounced { get; init; }

    /// <summary>
    /// True if we have ever scraped this tracker.
    /// </summary>
    public bool? HasScraped { get; init; }

    /// <summary>
    /// The tracker's host name.
    /// </summary>
    public string? Host { get; init; }

    /// <summary>
    /// The tracker's ID.
    /// </summary>
    public int? Id { get; init; }

    /// <summary>
    /// True if this is a backup tracker.
    /// </summary>
    public bool? IsBackup { get; init; }

    /// <summary>
    /// Number of peers returned in the last announce.
    /// </summary>
    public int? LastAnnouncePeerCount { get; init; }

    /// <summary>
    /// Result message of the last announce.
    /// </summary>
    public string? LastAnnounceResult { get; init; }

    /// <summary>
    /// The time the last announce started (unixtime).
    /// </summary>
    public int? LastAnnounceStartTime { get; init; }

    /// <summary>
    /// True if the last announce succeeded.
    /// </summary>
    public bool? LastAnnounceSucceeded { get; init; }

    /// <summary>
    /// The time of the last announce (unixtime).
    /// </summary>
    public int? LastAnnounceTime { get; init; }

    /// <summary>
    /// True if the last announce timed out.
    /// </summary>
    public bool? LastAnnounceTimedOut { get; init; }

    /// <summary>
    /// Result message of the last scrape.
    /// </summary>
    public string? LastScrapeResult { get; init; }

    /// <summary>
    /// The time the last scrape started (unixtime).
    /// </summary>
    public int? LastScrapeStartTime { get; init; }

    /// <summary>
    /// True if the last scrape succeeded.
    /// </summary>
    public bool? LastScrapeSucceeded { get; init; }

    /// <summary>
    /// The time of the last scrape (unixtime).
    /// </summary>
    public int? LastScrapeTime { get; init; }

    /// <summary>
    /// True if the last scrape timed out.
    /// </summary>
    public bool? LastScrapeTimedOut { get; init; }

    /// <summary>
    /// Number of leechers currently reported by the tracker.
    /// </summary>
    public int? LeecherCount { get; init; }

    /// <summary>
    /// The next time an announce will be sent (unixtime).
    /// </summary>
    public int? NextAnnounceTime { get; init; }

    /// <summary>
    /// The next time a scrape will be sent (unixtime).
    /// </summary>
    public int? NextScrapeTime { get; init; }

    /// <summary>
    /// The tracker's scrape URL.
    /// </summary>
    public string? Scrape { get; init; }

    /// <summary>
    /// The current state of scraping from this tracker.
    /// </summary>
    public int? ScrapeState { get; init; }

    /// <summary>
    /// Number of seeders currently reported by the tracker.
    /// </summary>
    public int? SeederCount { get; init; }

    /// <summary>
    /// The host name of the tracker.
    /// </summary>
    public string? Sitename { get; init; }

    /// <summary>
    /// The tracker's tier.
    /// </summary>
    public int? Tier { get; init; }
}

/// <summary>
/// Extended information about a webseed.
/// </summary>
public record WebseedEx
{
    /// <summary>
    /// The webseed's URL.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// True if we're currently downloading from this webseed.
    /// </summary>
    public bool? IsDownloading { get; init; }

    /// <summary>
    /// Download speed from this webseed (B/s).
    /// </summary>
    public int? DownloadBytesPerSecond { get; init; }
}
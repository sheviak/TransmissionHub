using TransmissionHub.Client.Models.Enums;

namespace TransmissionHub.Client.Models.Responses;

/// <summary>
/// Response containing session properties.
/// </summary>
public record SessionGetResponse
{
    /// <summary>
    /// Max global download speed (KBps).
    /// </summary>
    public int? AltSpeedDown { get; init; }

    /// <summary>
    /// True means use the alt speeds.
    /// </summary>
    public bool? AltSpeedEnabled { get; init; }

    /// <summary>
    /// When to turn on alt speeds (units: minutes after midnight).
    /// </summary>
    public int? AltSpeedTimeBegin { get; init; }

    /// <summary>
    /// What day(s) to turn on alt speeds.
    /// </summary>
    public int? AltSpeedTimeDay { get; init; }

    /// <summary>
    /// True means the scheduled on/off times are used.
    /// </summary>
    public bool? AltSpeedTimeEnabled { get; init; }

    /// <summary>
    /// When to turn off alt speeds.
    /// </summary>
    public int? AltSpeedTimeEnd { get; init; }

    /// <summary>
    /// Max global upload speed (KBps).
    /// </summary>
    public int? AltSpeedUp { get; init; }

    /// <summary>
    /// True means to enable a basic brute force protection for RPC server.
    /// </summary>
    public bool? AntiBruteForceEnabled { get; init; }

    /// <summary>
    /// True means blocklist is enabled.
    /// </summary>
    public bool? BlocklistEnabled { get; init; }

    /// <summary>
    /// Number of rules in the blocklist.
    /// </summary>
    public int? BlocklistSize { get; init; }

    /// <summary>
    /// Location of the blocklist.
    /// </summary>
    public string? BlocklistUrl { get; init; }

    /// <summary>
    /// Maximum size of the disk cache (MB).
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC version 18. Use <see cref="CacheSizeMib"/>.
    /// </remarks>
    public int? CacheSizeMb { get; init; }

    /// <summary>
    /// Maximum size of the disk cache (MiB).
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18. Deprecated in Transmission 4.2.0.
    /// </remarks>
    public int? CacheSizeMib { get; init; }

    /// <summary>
    /// Configured path for Transmission.
    /// </summary>
    public string? ConfigDir { get; init; }

    /// <summary>
    /// Announce URLs, one per line, and a blank line between tiers.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public string? DefaultTrackers { get; init; }

    /// <summary>
    /// True means allow DHT in public torrents.
    /// </summary>
    public bool? DhtEnabled { get; init; }

    /// <summary>
    /// Default path to download torrents.
    /// </summary>
    public string? DownloadDir { get; init; }

    /// <summary>
    /// Number of bytes free in the download directory.
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC version 17. Use free-space request instead.
    /// </remarks>
    public long? DownloadDirFreeSpace { get; init; }

    /// <summary>
    /// If true, limit how many torrents can be downloaded at once.
    /// </summary>
    public bool? DownloadQueueEnabled { get; init; }

    /// <summary>
    /// Max number of torrents to download at once.
    /// </summary>
    public int? DownloadQueueSize { get; init; }

    /// <summary>
    /// Encryption preference.
    /// </summary>
    public EncryptionMode? Encryption { get; init; }

    /// <summary>
    /// Torrents we're seeding will be stopped if they're idle for this long.
    /// </summary>
    public int? IdleSeedingLimit { get; init; }

    /// <summary>
    /// True if the seeding inactivity limit is honored by default.
    /// </summary>
    public bool? IdleSeedingLimitEnabled { get; init; }

    /// <summary>
    /// Path for incomplete torrents, when enabled.
    /// </summary>
    public string? IncompleteDir { get; init; }

    /// <summary>
    /// True means keep torrents in incomplete-dir until done.
    /// </summary>
    public bool? IncompleteDirEnabled { get; init; }

    /// <summary>
    /// True means allow Local Peer Discovery in public torrents.
    /// </summary>
    public bool? LpdEnabled { get; init; }

    /// <summary>
    /// Maximum global number of peers.
    /// </summary>
    public int? PeerLimitGlobal { get; init; }

    /// <summary>
    /// Maximum number of peers per torrent.
    /// </summary>
    public int? PeerLimitPerTorrent { get; init; }

    /// <summary>
    /// True means pick a random peer port on launch.
    /// </summary>
    public bool? PeerPortRandomOnStart { get; init; }

    /// <summary>
    /// Peer port number.
    /// </summary>
    public int? PeerPort { get; init; }

    /// <summary>
    /// True means allow PEX in public torrents.
    /// </summary>
    public bool? PexEnabled { get; init; }

    /// <summary>
    /// True means ask upstream router to forward the configured peer port.
    /// </summary>
    public bool? PortForwardingEnabled { get; init; }

    /// <summary>
    /// Preference of transport protocols.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18.
    /// </remarks>
    public IReadOnlyList<string>? PreferredTransports { get; init; }

    /// <summary>
    /// Whether or not to consider idle torrents as stalled.
    /// </summary>
    public bool? QueueStalledEnabled { get; init; }

    /// <summary>
    /// Torrents that are idle for N minutes aren't counted toward queue sizes.
    /// </summary>
    public int? QueueStalledMinutes { get; init; }

    /// <summary>
    /// True means append '.part' to incomplete files.
    /// </summary>
    public bool? RenamePartialFiles { get; init; }

    /// <summary>
    /// The number of outstanding block requests a peer is allowed to queue.
    /// </summary>
    public int? Reqq { get; init; }

    /// <summary>
    /// Minimum supported RPC version.
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC version 18. Use <see cref="RpcVersionSemver"/>.
    /// </remarks>
    public int? RpcVersionMinimum { get; init; }

    /// <summary>
    /// Semver formatting of the RPC version.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public string? RpcVersionSemver { get; init; }

    /// <summary>
    /// Current supported RPC version.
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC version 18. Use <see cref="RpcVersionSemver"/>.
    /// </remarks>
    public int? RpcVersion { get; init; }

    /// <summary>
    /// Whether or not to call the added script.
    /// </summary>
    public bool? ScriptTorrentAddedEnabled { get; init; }

    /// <summary>
    /// Filename of the script to run.
    /// </summary>
    public string? ScriptTorrentAddedFilename { get; init; }

    /// <summary>
    /// Whether or not to call the done script.
    /// </summary>
    public bool? ScriptTorrentDoneEnabled { get; init; }

    /// <summary>
    /// Filename of the script to run.
    /// </summary>
    public string? ScriptTorrentDoneFilename { get; init; }

    /// <summary>
    /// Whether or not to call the seeding-done script.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public bool? ScriptTorrentDoneSeedingEnabled { get; init; }

    /// <summary>
    /// Filename of the script to run.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public string? ScriptTorrentDoneSeedingFilename { get; init; }

    /// <summary>
    /// If true, limit how many torrents can be uploaded at once.
    /// </summary>
    public bool? SeedQueueEnabled { get; init; }

    /// <summary>
    /// Max number of torrents to upload at once.
    /// </summary>
    public int? SeedQueueSize { get; init; }

    /// <summary>
    /// The default seed ratio for torrents to use.
    /// </summary>
    public double? SeedRatioLimit { get; init; }

    /// <summary>
    /// True if seedRatioLimit is honored by default.
    /// </summary>
    public bool? SeedRatioLimited { get; init; }

    /// <summary>
    /// True means sequential download is enabled by default.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 18.
    /// </remarks>
    public bool? SequentialDownload { get; init; }

    /// <summary>
    /// Session Identifier string.
    /// </summary>
    public string? SessionId { get; init; }

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

    /// <summary>
    /// True means added torrents will be started right away.
    /// </summary>
    public bool? StartAddedTorrents { get; init; }

    /// <summary>
    /// True means allow TCP.
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC version 18. Use <see cref="PreferredTransports"/>.
    /// </remarks>
    public bool? TcpEnabled { get; init; }

    /// <summary>
    /// True means the .torrent file of added torrents will be deleted.
    /// </summary>
    public bool? TrashOriginalTorrentFiles { get; init; }

    /// <summary>
    /// Information about unit strings and byte limits.
    /// </summary>
    public UnitsInfo? Units { get; init; }

    /// <summary>
    /// True means allow UTP.
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC version 18. Use <see cref="PreferredTransports"/>.
    /// </remarks>
    public bool? UtpEnabled { get; init; }

    /// <summary>
    /// Transmission version.
    /// </summary>
    public string? Version { get; init; }
}

/// <summary>
/// Contains unit strings and byte limits.
/// </summary>
public record UnitsInfo
{
    /// <summary>
    /// 4 strings: KB/s, MB/s, GB/s, TB/s.
    /// </summary>
    public IReadOnlyList<string>? SpeedUnits { get; init; }

    /// <summary>
    /// Number of bytes in a KB (ie: 1000).
    /// </summary>
    public int? SpeedBytes { get; init; }

    /// <summary>
    /// 4 strings: KB, MB, GB, TB.
    /// </summary>
    public IReadOnlyList<string>? SizeUnits { get; init; }

    /// <summary>
    /// Number of bytes in a KB (ie: 1000).
    /// </summary>
    public int? SizeBytes { get; init; }

    /// <summary>
    /// 4 strings: KB, MB, GB, TB.
    /// </summary>
    public IReadOnlyList<string>? MemoryUnits { get; init; }

    /// <summary>
    /// Number of bytes in a KB (ie: 1024).
    /// </summary>
    public int? MemoryBytes { get; init; }
}
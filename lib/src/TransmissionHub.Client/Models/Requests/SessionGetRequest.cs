using System.Diagnostics.CodeAnalysis;

namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// Request to get session properties.
/// </summary>
public record SessionGetRequest
{
    /// <summary>
    /// Array of fields to return. If omitted, all fields are returned.
    /// </summary>
    public IReadOnlyList<string>? Fields { get; init; }

    /// <summary>
    /// Constants for all possible session fields.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("Style", "IDE1006:Naming Styles")]
    public static class SessionFields
    {
        /// <summary>
        /// Field AltSpeedDown.
        /// </summary>
        public const string AltSpeedDown = nameof(AltSpeedDown);

        /// <summary>
        /// Field AltSpeedEnabled.
        /// </summary>
        public const string AltSpeedEnabled = nameof(AltSpeedEnabled);

        /// <summary>
        /// Field AltSpeedTimeBegin.
        /// </summary>
        public const string AltSpeedTimeBegin = nameof(AltSpeedTimeBegin);

        /// <summary>
        /// Field AltSpeedTimeDay.
        /// </summary>
        public const string AltSpeedTimeDay = nameof(AltSpeedTimeDay);

        /// <summary>
        /// Field AltSpeedTimeEnabled.
        /// </summary>
        public const string AltSpeedTimeEnabled = nameof(AltSpeedTimeEnabled);

        /// <summary>
        /// Field AltSpeedTimeEnd.
        /// </summary>
        public const string AltSpeedTimeEnd = nameof(AltSpeedTimeEnd);

        /// <summary>
        /// Field AltSpeedUp.
        /// </summary>
        public const string AltSpeedUp = nameof(AltSpeedUp);

        /// <summary>
        /// Field AntiBruteForceEnabled.
        /// </summary>
        public const string AntiBruteForceEnabled = nameof(AntiBruteForceEnabled);

        /// <summary>
        /// Field BlocklistEnabled.
        /// </summary>
        public const string BlocklistEnabled = nameof(BlocklistEnabled);

        /// <summary>
        /// Field BlocklistSize.
        /// </summary>
        /// <remarks>
        /// <b>Readonly</b>
        /// </remarks>
        public const string BlocklistSize = nameof(BlocklistSize);

        /// <summary>
        /// Field BlocklistUrl.
        /// </summary>
        public const string BlocklistUrl = nameof(BlocklistUrl);

        /// <summary>
        /// Field CacheSizeMb.
        /// </summary>
        /// <remarks>
        /// Deprecated in RPC version 18. Use <see cref="CacheSizeMib"/>.
        /// </remarks>
        public const string CacheSizeMb = nameof(CacheSizeMb);

        /// <summary>
        /// Field CacheSizeMib.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 18. Deprecated in RPC version 19/Transmission 4.2.0.
        /// </remarks>
        public const string CacheSizeMib = nameof(CacheSizeMib);

        /// <summary>
        /// Field ConfigDir.
        /// </summary>
        /// <remarks>
        /// <b>Readonly</b>
        /// </remarks>
        public const string ConfigDir = nameof(ConfigDir);

        /// <summary>
        /// Field DefaultTrackers.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 17.
        /// </remarks>
        public const string DefaultTrackers = nameof(DefaultTrackers);

        /// <summary>
        /// Field DhtEnabled.
        /// </summary>
        public const string DhtEnabled = nameof(DhtEnabled);

        /// <summary>
        /// Field DownloadDir.
        /// </summary>
        public const string DownloadDir = nameof(DownloadDir);

        /// <summary>
        /// Field DownloadDirFreeSpace.
        /// </summary>
        /// <remarks>
        /// <b>Readonly</b> Deprecated in RPC version 17. Use free-space method instead.
        /// </remarks>
        public const string DownloadDirFreeSpace = nameof(DownloadDirFreeSpace);

        /// <summary>
        /// Field DownloadQueueEnabled.
        /// </summary>
        public const string DownloadQueueEnabled = nameof(DownloadQueueEnabled);

        /// <summary>
        /// Field DownloadQueueSize.
        /// </summary>
        public const string DownloadQueueSize = nameof(DownloadQueueSize);

        /// <summary>
        /// Field Encryption.
        /// </summary>
        public const string Encryption = nameof(Encryption);

        /// <summary>
        /// Field IdleSeedingLimit.
        /// </summary>
        public const string IdleSeedingLimit = nameof(IdleSeedingLimit);

        /// <summary>
        /// Field IdleSeedingLimitEnabled.
        /// </summary>
        public const string IdleSeedingLimitEnabled = nameof(IdleSeedingLimitEnabled);

        /// <summary>
        /// Field IncompleteDir.
        /// </summary>
        public const string IncompleteDir = nameof(IncompleteDir);

        /// <summary>
        /// Field IncompleteDirEnabled.
        /// </summary>
        public const string IncompleteDirEnabled = nameof(IncompleteDirEnabled);

        /// <summary>
        /// Field LpdEnabled.
        /// </summary>
        public const string LpdEnabled = nameof(LpdEnabled);

        /// <summary>
        /// Field PeerLimitGlobal.
        /// </summary>
        public const string PeerLimitGlobal = nameof(PeerLimitGlobal);

        /// <summary>
        /// Field PeerLimitPerTorrent.
        /// </summary>
        public const string PeerLimitPerTorrent = nameof(PeerLimitPerTorrent);

        /// <summary>
        /// Field PeerPortRandomOnStart.
        /// </summary>
        public const string PeerPortRandomOnStart = nameof(PeerPortRandomOnStart);

        /// <summary>
        /// Field PeerPort.
        /// </summary>
        public const string PeerPort = nameof(PeerPort);

        /// <summary>
        /// Field PexEnabled.
        /// </summary>
        public const string PexEnabled = nameof(PexEnabled);

        /// <summary>
        /// Field PortForwardingEnabled.
        /// </summary>
        public const string PortForwardingEnabled = nameof(PortForwardingEnabled);

        /// <summary>
        /// Field PreferredTransports.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 18.
        /// </remarks>
        public const string PreferredTransports = nameof(PreferredTransports);

        /// <summary>
        /// Field QueueStalledEnabled.
        /// </summary>
        public const string QueueStalledEnabled = nameof(QueueStalledEnabled);

        /// <summary>
        /// Field QueueStalledMinutes.
        /// </summary>
        public const string QueueStalledMinutes = nameof(QueueStalledMinutes);

        /// <summary>
        /// Field RenamePartialFiles.
        /// </summary>
        public const string RenamePartialFiles = nameof(RenamePartialFiles);

        /// <summary>
        /// Field Reqq.
        /// </summary>
        public const string Reqq = nameof(Reqq);

        /// <summary>
        /// Field RpcVersionMinimum.
        /// </summary>
        /// <remarks>
        /// <b>Readonly</b> Deprecated in RPC version 18.
        /// </remarks>
        public const string RpcVersionMinimum = nameof(RpcVersionMinimum);

        /// <summary>
        /// Field RpcVersionSemver.
        /// </summary>
        /// <remarks>
        /// <b>Readonly</b> Added in RPC version 17.
        /// </remarks>
        public const string RpcVersionSemver = nameof(RpcVersionSemver);

        /// <summary>
        /// Field RpcVersion.
        /// </summary>
        /// <remarks>
        /// <b>Readonly</b> Deprecated in RPC version 18.
        /// </remarks>
        public const string RpcVersion = nameof(RpcVersion);

        /// <summary>
        /// Field ScriptTorrentAddedEnabled.
        /// </summary>
        public const string ScriptTorrentAddedEnabled = nameof(ScriptTorrentAddedEnabled);

        /// <summary>
        /// Field ScriptTorrentAddedFilename.
        /// </summary>
        public const string ScriptTorrentAddedFilename = nameof(ScriptTorrentAddedFilename);

        /// <summary>
        /// Field ScriptTorrentDoneEnabled.
        /// </summary>
        public const string ScriptTorrentDoneEnabled = nameof(ScriptTorrentDoneEnabled);

        /// <summary>
        /// Field ScriptTorrentDoneFilename.
        /// </summary>
        public const string ScriptTorrentDoneFilename = nameof(ScriptTorrentDoneFilename);

        /// <summary>
        /// Field ScriptTorrentDoneSeedingEnabled.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 17.
        /// </remarks>
        public const string ScriptTorrentDoneSeedingEnabled = nameof(ScriptTorrentDoneSeedingEnabled);

        /// <summary>
        /// Field ScriptTorrentDoneSeedingFilename.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 17.
        /// </remarks>
        public const string ScriptTorrentDoneSeedingFilename = nameof(ScriptTorrentDoneSeedingFilename);

        /// <summary>
        /// Field SeedQueueEnabled.
        /// </summary>
        public const string SeedQueueEnabled = nameof(SeedQueueEnabled);

        /// <summary>
        /// Field SeedQueueSize.
        /// </summary>
        public const string SeedQueueSize = nameof(SeedQueueSize);

        /// <summary>
        /// Field SeedRatioLimit.
        /// </summary>
        public const string SeedRatioLimit = nameof(SeedRatioLimit);

        /// <summary>
        /// Field SeedRatioLimited.
        /// </summary>
        public const string SeedRatioLimited = nameof(SeedRatioLimited);

        /// <summary>
        /// Field SequentialDownload.
        /// </summary>
        public const string SequentialDownload = nameof(SequentialDownload);

        /// <summary>
        /// Field SessionId.
        /// </summary>
        /// <remarks>
        /// <b>Readonly</b>
        /// </remarks>
        public const string SessionId = nameof(SessionId);

        /// <summary>
        /// Field SpeedLimitDown.
        /// </summary>
        public const string SpeedLimitDown = nameof(SpeedLimitDown);

        /// <summary>
        /// Field SpeedLimitDownEnabled.
        /// </summary>
        public const string SpeedLimitDownEnabled = nameof(SpeedLimitDownEnabled);

        /// <summary>
        /// Field SpeedLimitUp.
        /// </summary>
        public const string SpeedLimitUp = nameof(SpeedLimitUp);

        /// <summary>
        /// Field SpeedLimitUpEnabled.
        /// </summary>
        public const string SpeedLimitUpEnabled = nameof(SpeedLimitUpEnabled);

        /// <summary>
        /// Field StartAddedTorrents.
        /// </summary>
        public const string StartAddedTorrents = nameof(StartAddedTorrents);

        /// <summary>
        /// Field TcpEnabled.
        /// </summary>
        /// <remarks>
        /// Deprecated in RPC version 18. Use <see cref="PreferredTransports"/>.
        /// </remarks>
        public const string TcpEnabled = nameof(TcpEnabled);

        /// <summary>
        /// Field TrashOriginalTorrentFiles.
        /// </summary>
        public const string TrashOriginalTorrentFiles = nameof(TrashOriginalTorrentFiles);

        /// <summary>
        /// Field Units.
        /// </summary>
        /// <remarks>
        /// <b>Readonly</b>
        /// </remarks>
        public const string Units = nameof(Units);

        /// <summary>
        /// Field UtpEnabled.
        /// </summary>
        /// <remarks>
        /// Deprecated in RPC version 18. Use <see cref="PreferredTransports"/>.
        /// </remarks>
        public const string UtpEnabled = nameof(UtpEnabled);

        /// <summary>
        /// Field Version.
        /// </summary>
        /// <remarks>
        /// <b>Readonly</b>
        /// </remarks>
        public const string Version = nameof(Version);

        /// <summary>
        /// All session fields.
        /// </summary>
        public static readonly IReadOnlyList<string> All =
        [
            AltSpeedDown, AltSpeedEnabled, AltSpeedTimeBegin, AltSpeedTimeDay,
            AltSpeedTimeEnabled, AltSpeedTimeEnd, AltSpeedUp, AntiBruteForceEnabled,
            BlocklistEnabled, BlocklistSize, BlocklistUrl, CacheSizeMb, CacheSizeMib,
            ConfigDir, DefaultTrackers, DhtEnabled, DownloadDir, DownloadDirFreeSpace,
            DownloadQueueEnabled, DownloadQueueSize, Encryption, IdleSeedingLimit,
            IdleSeedingLimitEnabled, IncompleteDir, IncompleteDirEnabled, LpdEnabled,
            PeerLimitGlobal, PeerLimitPerTorrent, PeerPortRandomOnStart, PeerPort,
            PexEnabled, PortForwardingEnabled, PreferredTransports, QueueStalledEnabled,
            QueueStalledMinutes, RenamePartialFiles, Reqq, RpcVersionMinimum,
            RpcVersionSemver, RpcVersion, ScriptTorrentAddedEnabled, ScriptTorrentAddedFilename,
            ScriptTorrentDoneEnabled, ScriptTorrentDoneFilename, ScriptTorrentDoneSeedingEnabled,
            ScriptTorrentDoneSeedingFilename, SeedQueueEnabled, SeedQueueSize,
            SeedRatioLimit, SeedRatioLimited, SequentialDownload, SessionId,
            SpeedLimitDown, SpeedLimitDownEnabled, SpeedLimitUp, SpeedLimitUpEnabled,
            StartAddedTorrents, TcpEnabled, TrashOriginalTorrentFiles, Units,
            UtpEnabled, Version
        ];

        /// <summary>
        /// Gets commonly used session fields.
        /// </summary>
        public static readonly string[] Common =
        [
            Version, RpcVersion, RpcVersionSemver, DownloadDir, DownloadDirFreeSpace, PeerPort, PeerPortRandomOnStart,
            DhtEnabled, PexEnabled, LpdEnabled, UtpEnabled, PortForwardingEnabled, Encryption, SpeedLimitDown,
            SpeedLimitDownEnabled, SpeedLimitUp, SpeedLimitUpEnabled, StartAddedTorrents,
        ];

        /// <summary>
        /// Gets speed and ratio related fields.
        /// </summary>
        public static readonly string[] SpeedAndRatio =
        [
            AltSpeedUp, AltSpeedDown, AltSpeedEnabled, AltSpeedTimeBegin, AltSpeedTimeDay, AltSpeedTimeEnabled,
            AltSpeedTimeEnd, SpeedLimitDown, SpeedLimitDownEnabled, SpeedLimitUp, SpeedLimitUpEnabled, SeedRatioLimit,
            SeedRatioLimited,
        ];

        /// <summary>
        /// Gets queue-related session fields.
        /// </summary>
        public static readonly string[] Queue =
        [
            DownloadQueueEnabled, DownloadQueueSize,
            SeedQueueEnabled, SeedQueueSize,
            QueueStalledEnabled, QueueStalledMinutes,
        ];

        /// <summary>
        /// Gets script-related session fields.
        /// </summary>
        public static readonly string[] Scripts =
        [
            ScriptTorrentAddedFilename,
            ScriptTorrentAddedEnabled,
            ScriptTorrentDoneSeedingFilename,
            ScriptTorrentDoneSeedingEnabled,
            ScriptTorrentDoneFilename,
            ScriptTorrentDoneEnabled,
        ];
    }
}
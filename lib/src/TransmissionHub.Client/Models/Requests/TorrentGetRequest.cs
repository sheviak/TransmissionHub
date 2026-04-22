using System.Diagnostics.CodeAnalysis;

namespace TransmissionHub.Client.Models.Requests;
/// <summary>
/// Request to get torrent properties.
/// </summary>
public record TorrentGetRequest
{
    /// <summary>
    /// Specifies which torrents to use.
    /// </summary>
    public IReadOnlyList<TorrentId>? Ids { get; init; }

    /// <summary>
    /// Array of fields to return.
    /// </summary>
    public IReadOnlyList<string>? Fields { get; init; }

    /// <summary>
    /// Specifies how to format the torrents response field. Allowed values are 'objects' (default) and 'table'.
    /// </summary>
    public string? Format { get; init; }

    /// <summary>
    /// Constants for all possible torrent fields.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("Style", "IDE1006:Naming Styles")]
    public static class TorrentFields
    {
        /// <summary>
        /// Field ActivityDate.
        /// </summary>
        public const string ActivityDate = nameof(ActivityDate);

        /// <summary>
        /// Field AddedDate.
        /// </summary>
        public const string AddedDate = nameof(AddedDate);

        /// <summary>
        /// Field Availability.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 17.
        /// </remarks>
        public const string Availability = nameof(Availability);

        /// <summary>
        /// Field BandwidthPriority.
        /// </summary>
        public const string BandwidthPriority = nameof(BandwidthPriority);

        /// <summary>
        /// Field BytesCompleted.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 18.
        /// </remarks>
        public const string BytesCompleted = nameof(BytesCompleted);

        /// <summary>
        /// Field Comment.
        /// </summary>
        public const string Comment = nameof(Comment);

        /// <summary>
        /// Field CorruptEver.
        /// </summary>
        public const string CorruptEver = nameof(CorruptEver);

        /// <summary>
        /// Field Creator.
        /// </summary>
        public const string Creator = nameof(Creator);

        /// <summary>
        /// Field DateCreated.
        /// </summary>
        public const string DateCreated = nameof(DateCreated);

        /// <summary>
        /// Field DesiredAvailable.
        /// </summary>
        public const string DesiredAvailable = nameof(DesiredAvailable);

        /// <summary>
        /// Field DoneDate.
        /// </summary>
        public const string DoneDate = nameof(DoneDate);

        /// <summary>
        /// Field DownloadDir.
        /// </summary>
        public const string DownloadDir = nameof(DownloadDir);

        /// <summary>
        /// Field DownloadedEver.
        /// </summary>
        public const string DownloadedEver = nameof(DownloadedEver);

        /// <summary>
        /// Field DownloadLimit.
        /// </summary>
        public const string DownloadLimit = nameof(DownloadLimit);

        /// <summary>
        /// Field DownloadLimited.
        /// </summary>
        public const string DownloadLimited = nameof(DownloadLimited);

        /// <summary>
        /// Field EditDate.
        /// </summary>
        public const string EditDate = nameof(EditDate);

        /// <summary>
        /// Field Error.
        /// </summary>
        public const string Error = nameof(Error);

        /// <summary>
        /// Field ErrorString.
        /// </summary>
        public const string ErrorString = nameof(ErrorString);

        /// <summary>
        /// Field Eta.
        /// </summary>
        public const string Eta = nameof(Eta);

        /// <summary>
        /// Field EtaIdle.
        /// </summary>
        public const string EtaIdle = nameof(EtaIdle);

        /// <summary>
        /// Field FileCount.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 17.
        /// </remarks>
        public const string FileCount = nameof(FileCount);

        /// <summary>
        /// Field Files.
        /// </summary>
        public const string Files = nameof(Files);

        /// <summary>
        /// Field FileStats.
        /// </summary>
        public const string FileStats = nameof(FileStats);

        /// <summary>
        /// Field Group.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 17.
        /// </remarks>
        public const string Group = nameof(Group);

        /// <summary>
        /// Field HashString.
        /// </summary>
        public const string HashString = nameof(HashString);

        /// <summary>
        /// Field HaveUnchecked.
        /// </summary>
        public const string HaveUnchecked = nameof(HaveUnchecked);

        /// <summary>
        /// Field HaveValid.
        /// </summary>
        public const string HaveValid = nameof(HaveValid);

        /// <summary>
        /// Field HonorsSessionLimits.
        /// </summary>
        public const string HonorsSessionLimits = nameof(HonorsSessionLimits);

        /// <summary>
        /// Field Id.
        /// </summary>
        public const string Id = nameof(Id);

        /// <summary>
        /// Field IsFinished.
        /// </summary>
        public const string IsFinished = nameof(IsFinished);

        /// <summary>
        /// Field IsPrivate.
        /// </summary>
        public const string IsPrivate = nameof(IsPrivate);

        /// <summary>
        /// Field IsStalled.
        /// </summary>
        public const string IsStalled = nameof(IsStalled);

        /// <summary>
        /// Field Labels.
        /// </summary>
        public const string Labels = nameof(Labels);

        /// <summary>
        /// Field LeftUntilDone.
        /// </summary>
        public const string LeftUntilDone = nameof(LeftUntilDone);

        /// <summary>
        /// Field MagnetLink.
        /// </summary>
        public const string MagnetLink = nameof(MagnetLink);

        /// <summary>
        /// Field ManualAnnounceTime.
        /// </summary>
        /// <remarks>
        /// Deprecated in RPC version 18. Never worked.
        /// </remarks>
        public const string ManualAnnounceTime = nameof(ManualAnnounceTime);

        /// <summary>
        /// Field MaxConnectedPeers.
        /// </summary>
        public const string MaxConnectedPeers = nameof(MaxConnectedPeers);

        /// <summary>
        /// Field MetadataPercentComplete.
        /// </summary>
        public const string MetadataPercentComplete = nameof(MetadataPercentComplete);

        /// <summary>
        /// Field Name.
        /// </summary>
        public const string Name = nameof(Name);

        /// <summary>
        /// Field PeerLimit.
        /// </summary>
        public const string PeerLimit = nameof(PeerLimit);

        /// <summary>
        /// Field Peers.
        /// </summary>
        public const string Peers = nameof(Peers);

        /// <summary>
        /// Field PeersConnected.
        /// </summary>
        public const string PeersConnected = nameof(PeersConnected);

        /// <summary>
        /// Field PeersFrom.
        /// </summary>
        public const string PeersFrom = nameof(PeersFrom);

        /// <summary>
        /// Field PeersGettingFromUs.
        /// </summary>
        public const string PeersGettingFromUs = nameof(PeersGettingFromUs);

        /// <summary>
        /// Field PeersSendingToUs.
        /// </summary>
        public const string PeersSendingToUs = nameof(PeersSendingToUs);

        /// <summary>
        /// Field PercentComplete.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 17.
        /// </remarks>
        public const string PercentComplete = nameof(PercentComplete);

        /// <summary>
        /// Field PercentDone.
        /// </summary>
        public const string PercentDone = nameof(PercentDone);

        /// <summary>
        /// Field Pieces.
        /// </summary>
        public const string Pieces = nameof(Pieces);

        /// <summary>
        /// Field PieceCount.
        /// </summary>
        public const string PieceCount = nameof(PieceCount);

        /// <summary>
        /// Field PieceSize.
        /// </summary>
        public const string PieceSize = nameof(PieceSize);

        /// <summary>
        /// Field Priorities.
        /// </summary>
        public const string Priorities = nameof(Priorities);

        /// <summary>
        /// Field PrimaryMimeType.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 17.
        /// </remarks>
        public const string PrimaryMimeType = nameof(PrimaryMimeType);

        /// <summary>
        /// Field QueuePosition.
        /// </summary>
        public const string QueuePosition = nameof(QueuePosition);

        /// <summary>
        /// Field RateDownload.
        /// </summary>
        public const string RateDownload = nameof(RateDownload);

        /// <summary>
        /// Field RateUpload.
        /// </summary>
        public const string RateUpload = nameof(RateUpload);

        /// <summary>
        /// Field RecheckProgress.
        /// </summary>
        public const string RecheckProgress = nameof(RecheckProgress);

        /// <summary>
        /// Field SecondsDownloading.
        /// </summary>
        public const string SecondsDownloading = nameof(SecondsDownloading);

        /// <summary>
        /// Field SecondsSeeding.
        /// </summary>
        public const string SecondsSeeding = nameof(SecondsSeeding);

        /// <summary>
        /// Field SeedIdleLimit.
        /// </summary>
        public const string SeedIdleLimit = nameof(SeedIdleLimit);

        /// <summary>
        /// Field SeedIdleMode.
        /// </summary>
        public const string SeedIdleMode = nameof(SeedIdleMode);

        /// <summary>
        /// Field SeedRatioLimit.
        /// </summary>
        public const string SeedRatioLimit = nameof(SeedRatioLimit);

        /// <summary>
        /// Field SeedRatioMode.
        /// </summary>
        public const string SeedRatioMode = nameof(SeedRatioMode);

        /// <summary>
        /// Field SequentialDownload.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 18.
        /// </remarks>
        public const string SequentialDownload = nameof(SequentialDownload);

        /// <summary>
        /// Field SequentialDownloadFromPiece.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 18.
        /// </remarks>
        public const string SequentialDownloadFromPiece = nameof(SequentialDownloadFromPiece);

        /// <summary>
        /// Field SizeWhenDone.
        /// </summary>
        public const string SizeWhenDone = nameof(SizeWhenDone);

        /// <summary>
        /// Field StartDate.
        /// </summary>
        public const string StartDate = nameof(StartDate);

        /// <summary>
        /// Field Status.
        /// </summary>
        public const string Status = nameof(Status);

        /// <summary>
        /// Field Trackers.
        /// </summary>
        public const string Trackers = nameof(Trackers);

        /// <summary>
        /// Field TrackerList.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 17.
        /// </remarks>
        public const string TrackerList = nameof(TrackerList);

        /// <summary>
        /// Field TrackerStats.
        /// </summary>
        public const string TrackerStats = nameof(TrackerStats);

        /// <summary>
        /// Field TotalSize.
        /// </summary>
        public const string TotalSize = nameof(TotalSize);

        /// <summary>
        /// Field TorrentFile.
        /// </summary>
        public const string TorrentFile = nameof(TorrentFile);

        /// <summary>
        /// Field UploadedEver.
        /// </summary>
        public const string UploadedEver = nameof(UploadedEver);

        /// <summary>
        /// Field UploadLimit.
        /// </summary>
        public const string UploadLimit = nameof(UploadLimit);

        /// <summary>
        /// Field UploadLimited.
        /// </summary>
        public const string UploadLimited = nameof(UploadLimited);

        /// <summary>
        /// Field UploadRatio.
        /// </summary>
        public const string UploadRatio = nameof(UploadRatio);

        /// <summary>
        /// Field Wanted.
        /// </summary>
        public const string Wanted = nameof(Wanted);

        /// <summary>
        /// Field Webseeds.
        /// </summary>
        /// <remarks>
        /// Deprecated in RPC version ? Use WebseedsEx instead.
        /// </remarks>
        public const string Webseeds = nameof(Webseeds);

        /// <summary>
        /// Field WebseedsEx.
        /// </summary>
        /// <remarks>
        /// Added in RPC version 19/Transmission 4.2.0.
        /// </remarks>
        public const string WebseedsEx = nameof(WebseedsEx);

        /// <summary>
        /// Field WebseedsSendingToUs.
        /// </summary>
        public const string WebseedsSendingToUs = nameof(WebseedsSendingToUs);

        /// <summary>
        /// Gets all available torrent fields in canonical notation.
        /// </summary>
        public static readonly IReadOnlyList<string> All =
        [
            ActivityDate, AddedDate, Availability, BandwidthPriority, BytesCompleted, Comment, CorruptEver, Creator,
            DateCreated, DesiredAvailable, DoneDate, DownloadDir, DownloadedEver, DownloadLimit, DownloadLimited,
            EditDate, Error, ErrorString, Eta, EtaIdle, FileCount, Files, FileStats, Group, HashString, HaveUnchecked,
            HaveValid, HonorsSessionLimits, Id, IsFinished, IsPrivate, IsStalled, Labels, LeftUntilDone, MagnetLink,
            ManualAnnounceTime, MaxConnectedPeers, MetadataPercentComplete, Name, PeerLimit, Peers, PeersConnected, PeersFrom,
            PeersGettingFromUs, PeersSendingToUs, PercentComplete, PercentDone, Pieces, PieceCount, PieceSize, Priorities,
            PrimaryMimeType, QueuePosition, RateDownload, RateUpload, RecheckProgress, SecondsDownloading, SecondsSeeding,
            SeedIdleLimit, SeedIdleMode, SeedRatioLimit, SeedRatioMode, SequentialDownload, SequentialDownloadFromPiece,
            SizeWhenDone, StartDate, Status, Trackers, TrackerList, TrackerStats, TotalSize, TorrentFile, UploadedEver,
            UploadLimit, UploadLimited, UploadRatio, Wanted, Webseeds, WebseedsEx, WebseedsSendingToUs
        ];

        /// <summary>
        /// Gets a minimal identity field set.
        /// </summary>
        public static readonly string[] Identity = [Id, Name, HashString];

        /// <summary>
        /// Gets a commonly used dashboard field set.
        /// </summary>
        public static readonly string[] Common =
        [
            Id, Name, HashString, Status, PercentDone, Eta, RateDownload, RateUpload, LeftUntilDone, SizeWhenDone,
            TotalSize, Error, ErrorString, QueuePosition, DownloadDir, Group,
        ];

        /// <summary>
        /// Gets a progress-oriented field set.
        /// </summary>
        public static readonly string[] Progress =
        [
            Id, Name, Status, PercentDone, PercentComplete, LeftUntilDone, SizeWhenDone, Eta, EtaIdle,
            RecheckProgress, IsFinished, IsStalled,
        ];

        /// <summary>
        /// Gets a transfer-oriented field set.
        /// </summary>
        public static readonly string[] Transfer =
        [
            Id, Name, RateDownload, RateUpload, DownloadedEver, UploadedEver, UploadRatio,
            PeersConnected, PeersSendingToUs, PeersGettingFromUs,
        ];

        /// <summary>
        /// Gets an extended details field set.
        /// </summary>
        public static readonly string[] Details =
        [
            Id, Name, HashString, Comment, Creator, DateCreated, MagnetLink, TorrentFile, Files, FileStats, Trackers,
            TrackerStats, Peers, Webseeds, Pieces, PieceCount, PieceSize, PrimaryMimeType,
        ];
    }
}
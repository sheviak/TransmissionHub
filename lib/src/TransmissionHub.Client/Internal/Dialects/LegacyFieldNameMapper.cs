namespace TransmissionHub.Client.Internal.Dialects;

/// <summary>
/// Maps field names between PascalCase and legacy wire format for RPC versions 16 and 17.
/// </summary>
/// <remarks>
/// The legacy Transmission RPC API uses an inconsistent mix of naming conventions:
/// most fields use <c>camelCase</c>, but some use <c>kebab-case</c>.
/// This mapper encapsulates the explicit override dictionary for request serialization.
/// <para>
/// This class is used only by <see cref="RpcRequestDialect"/> (v16/v17).
/// <see cref="JsonRpcDialect"/> (v18+) uses the built-in
/// <c>JsonNamingPolicy.SnakeCaseLower</c> without any custom mapping.
/// </para>
/// <para>
/// Response normalization (wire → snake_case) is handled separately by
/// <see cref="RpcPayloadKeyNormalizer"/>, which uniformly converts any key format to snake_case.
/// </para>
/// </remarks>
internal static class LegacyFieldNameMapper
{
    /// <summary>
    /// Explicit PascalCase → legacy wire-name overrides for fields that use kebab-case
    /// or other non-camelCase formats in the legacy API.
    /// Fields not in this dictionary fall back to camelCase (lowerFirst).
    /// </summary>
    private static readonly Dictionary<string, string> RequestOverrides = new(StringComparer.Ordinal)
    {
        // torrent-get / torrent-set / torrent-add
        ["FileCount"] = "file-count",
        ["PeerLimit"] = "peer-limit",
        ["PrimaryMimeType"] = "primary-mime-type",
        ["FilesWanted"] = "files-wanted",
        ["FilesUnwanted"] = "files-unwanted",
        ["PriorityHigh"] = "priority-high",
        ["PriorityLow"] = "priority-low",
        ["PriorityNormal"] = "priority-normal",
        ["DownloadDir"] = "download-dir",
        ["DeleteLocalData"] = "delete-local-data",

        // session-get / session-set (kebab-case session fields)
        ["AltSpeedDown"] = "alt-speed-down",
        ["AltSpeedEnabled"] = "alt-speed-enabled",
        ["AltSpeedTimeBegin"] = "alt-speed-time-begin",
        ["AltSpeedTimeDay"] = "alt-speed-time-day",
        ["AltSpeedTimeEnabled"] = "alt-speed-time-enabled",
        ["AltSpeedTimeEnd"] = "alt-speed-time-end",
        ["AltSpeedUp"] = "alt-speed-up",
        ["BlocklistEnabled"] = "blocklist-enabled",
        ["BlocklistSize"] = "blocklist-size",
        ["BlocklistUrl"] = "blocklist-url",
        ["CacheSizeMb"] = "cache-size-mb",
        ["ConfigDir"] = "config-dir",
        ["DefaultTrackers"] = "default-trackers",
        ["DhtEnabled"] = "dht-enabled",
        ["DownloadDirFreeSpace"] = "download-dir-free-space",
        ["DownloadQueueEnabled"] = "download-queue-enabled",
        ["DownloadQueueSize"] = "download-queue-size",
        ["IdleSeedingLimit"] = "idle-seeding-limit",
        ["IdleSeedingLimitEnabled"] = "idle-seeding-limit-enabled",
        ["IncompleteDirEnabled"] = "incomplete-dir-enabled",
        ["IncompleteDir"] = "incomplete-dir",
        ["LpdEnabled"] = "lpd-enabled",
        ["PeerLimitGlobal"] = "peer-limit-global",
        ["PeerLimitPerTorrent"] = "peer-limit-per-torrent",
        ["PeerPort"] = "peer-port",
        ["PeerPortRandomOnStart"] = "peer-port-random-on-start",
        ["PexEnabled"] = "pex-enabled",
        ["PortForwardingEnabled"] = "port-forwarding-enabled",
        ["QueueStalledEnabled"] = "queue-stalled-enabled",
        ["QueueStalledMinutes"] = "queue-stalled-minutes",
        ["RenamePartialFiles"] = "rename-partial-files",
        ["RpcVersion"] = "rpc-version",
        ["RpcVersionMinimum"] = "rpc-version-minimum",
        ["RpcVersionSemver"] = "rpc-version-semver",
        ["ScriptTorrentAddedEnabled"] = "script-torrent-added-enabled",
        ["ScriptTorrentAddedFilename"] = "script-torrent-added-filename",
        ["ScriptTorrentDoneEnabled"] = "script-torrent-done-enabled",
        ["ScriptTorrentDoneFilename"] = "script-torrent-done-filename",
        ["ScriptTorrentDoneSeedingEnabled"] = "script-torrent-done-seeding-enabled",
        ["ScriptTorrentDoneSeedingFilename"] = "script-torrent-done-seeding-filename",
        ["SeedQueueEnabled"] = "seed-queue-enabled",
        ["SeedQueueSize"] = "seed-queue-size",
        ["SpeedLimitDownEnabled"] = "speed-limit-down-enabled",
        ["SpeedLimitDown"] = "speed-limit-down",
        ["SpeedLimitUpEnabled"] = "speed-limit-up-enabled",
        ["SpeedLimitUp"] = "speed-limit-up",
        ["StartAddedTorrents"] = "start-added-torrents",
        ["TrashOriginalTorrentFiles"] = "trash-original-torrent-files",
        ["UtpEnabled"] = "utp-enabled",

        // session-stats (kebab-case nested objects)
        ["CumulativeStats"] = "cumulative-stats",
        ["CurrentStats"] = "current-stats",

        // free-space
        ["SizeBytes"] = "size-bytes",

        // port-test
        ["PortIsOpen"] = "port-is-open",
    };

    /// <summary>
    /// Converts a PascalCase field name to the legacy wire-format name.
    /// </summary>
    /// <remarks>
    /// If the field has an explicit override (e.g. <c>FileCount</c> → <c>file-count</c>),
    /// that override is returned. Otherwise, the name is converted to camelCase
    /// (lowercase first letter), which covers the majority of legacy fields.
    /// </remarks>
    /// <param name="pascalCase">The field name in PascalCase.</param>
    /// <returns>The field name in legacy wire format.</returns>
    public static string ToLegacyWireName(string pascalCase)
    {
        if (RequestOverrides.TryGetValue(pascalCase, out var wireName))
        {
            return wireName;
        }

        // Fallback: camelCase (most legacy fields use this convention)
        return char.ToLowerInvariant(pascalCase[0]) + pascalCase[1..];
    }
}
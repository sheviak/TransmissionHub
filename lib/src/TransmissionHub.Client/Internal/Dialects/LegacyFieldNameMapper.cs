namespace TransmissionHub.Client.Internal.Dialects;

/// <summary>
/// Maps field names between PascalCase and legacy wire format for RPC versions 16 and 17.
/// </summary>
/// <remarks>
/// The legacy Transmission RPC API uses an inconsistent mix of naming conventions,
/// and the same PascalCase name can map to different wire names depending on where
/// it is used: <c>DownloadDir</c> is <c>downloadDir</c> in the torrent-get field
/// table, but <c>download-dir</c> in torrent-add arguments and in session fields.
/// A single PascalCase → wire-name dictionary cannot express this, so the overrides
/// are split by scope, and callers pick the scope explicitly:
/// <list type="bullet">
///   <item><see cref="ToTorrentGetFieldName"/> — the <c>fields</c> array of torrent-get.</item>
///   <item><see cref="ToSessionFieldName"/> — the <c>fields</c> array of session-get and
///   the arguments of session-set (the wire names are identical).</item>
///   <item><see cref="ToArgumentName"/> — request argument bodies of all other methods
///   (torrent-add, torrent-set, torrent-remove, group-set).</item>
/// </list>
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
    /// Explicit PascalCase → wire-name overrides for the <c>fields</c> array of torrent-get.
    /// The torrent-get field table is camelCase with only these exceptions.
    /// </summary>
    private static readonly Dictionary<string, string> TorrentGetFieldOverrides = new(StringComparer.Ordinal)
    {
        // kebab-case exceptions of the torrent-get field table
        ["FileCount"] = "file-count",
        ["PeerLimit"] = "peer-limit",
        ["PrimaryMimeType"] = "primary-mime-type",

        // camelCase exception that diverges from every other scope:
        // torrent-get uses "downloadDir", while torrent-add arguments and session
        // fields use "download-dir" for the same PascalCase name.
        ["DownloadDir"] = "downloadDir",
    };

    /// <summary>
    /// Explicit PascalCase → wire-name overrides for session fields.
    /// Used for the <c>fields</c> array of session-get and the arguments of session-set,
    /// which share the same wire names.
    /// </summary>
    private static readonly Dictionary<string, string> SessionFieldOverrides = new(StringComparer.Ordinal)
    {
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
        ["DownloadDir"] = "download-dir",
        ["DownloadDirFreeSpace"] = "download-dir-free-space",
        ["DownloadQueueEnabled"] = "download-queue-enabled",
        ["DownloadQueueSize"] = "download-queue-size",
        ["IdleSeedingLimit"] = "idle-seeding-limit",
        ["IdleSeedingLimitEnabled"] = "idle-seeding-limit-enabled",
        ["IncompleteDir"] = "incomplete-dir",
        ["IncompleteDirEnabled"] = "incomplete-dir-enabled",
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
        ["SpeedLimitDown"] = "speed-limit-down",
        ["SpeedLimitDownEnabled"] = "speed-limit-down-enabled",
        ["SpeedLimitUp"] = "speed-limit-up",
        ["SpeedLimitUpEnabled"] = "speed-limit-up-enabled",
        ["StartAddedTorrents"] = "start-added-torrents",
        ["TrashOriginalTorrentFiles"] = "trash-original-torrent-files",
        ["UtpEnabled"] = "utp-enabled",
    };

    /// <summary>
    /// Explicit PascalCase → wire-name overrides for request argument bodies of
    /// torrent-scoped methods (torrent-add, torrent-set, torrent-remove, group-set).
    /// Session-set arguments are covered by <see cref="SessionFieldOverrides"/>, which
    /// <see cref="ToArgumentName"/> consults as a fallback.
    /// </summary>
    private static readonly Dictionary<string, string> ArgumentOverrides = new(StringComparer.Ordinal)
    {
        // torrent-add / torrent-set
        ["FilesWanted"] = "files-wanted",
        ["FilesUnwanted"] = "files-unwanted",
        ["PeerLimit"] = "peer-limit",
        ["PriorityHigh"] = "priority-high",
        ["PriorityLow"] = "priority-low",
        ["PriorityNormal"] = "priority-normal",

        // torrent-add
        ["DownloadDir"] = "download-dir",

        // torrent-remove
        ["DeleteLocalData"] = "delete-local-data",
    };

    /// <summary>
    /// Converts a PascalCase field name to its wire name in the <c>fields</c> array of torrent-get.
    /// </summary>
    /// <remarks>
    /// If the field has an explicit override (e.g. <c>FileCount</c> → <c>file-count</c>,
    /// <c>DownloadDir</c> → <c>downloadDir</c>), that override is returned.
    /// Otherwise, the name is converted to camelCase (lowercase first letter).
    /// </remarks>
    /// <param name="pascalCase">The field name in PascalCase.</param>
    /// <returns>The field name in torrent-get wire format.</returns>
    public static string ToTorrentGetFieldName(string pascalCase)
        => Resolve(pascalCase, TorrentGetFieldOverrides);

    /// <summary>
    /// Converts a PascalCase field name to its wire name in the <c>fields</c> array of
    /// session-get and the arguments of session-set.
    /// </summary>
    /// <remarks>
    /// If the field has an explicit override (e.g. <c>DownloadDir</c> → <c>download-dir</c>),
    /// that override is returned. Otherwise, the name is converted to camelCase.
    /// </remarks>
    /// <param name="pascalCase">The field name in PascalCase.</param>
    /// <returns>The field name in session wire format.</returns>
    public static string ToSessionFieldName(string pascalCase)
        => Resolve(pascalCase, SessionFieldOverrides);

    /// <summary>
    /// Converts a PascalCase property name to its wire name in a request argument body.
    /// </summary>
    /// <remarks>
    /// Torrent-scoped overrides are consulted first, then session overrides
    /// (session-set arguments share the session field wire names), then the name is
    /// converted to camelCase, which covers the majority of legacy fields.
    /// </remarks>
    /// <param name="pascalCase">The property name in PascalCase.</param>
    /// <returns>The property name in legacy wire format.</returns>
    public static string ToArgumentName(string pascalCase)
        => ArgumentOverrides.TryGetValue(pascalCase, out var wireName)
            ? wireName
            : Resolve(pascalCase, SessionFieldOverrides);

    /// <summary>
    /// Resolves <paramref name="pascalCase"/> against the provided override dictionaries in order,
    /// falling back to camelCase (lowercase first letter).
    /// </summary>
    /// <param name="pascalCase">The field name in PascalCase.</param>
    /// <param name="overrides">The override dictionaries to consult, in precedence order.</param>
    /// <returns>The resolved wire name.</returns>
    private static string Resolve(string pascalCase, params Dictionary<string, string>[] overrides)
    {
        foreach (var map in overrides)
        {
            if (map.TryGetValue(pascalCase, out var wireName))
            {
                return wireName;
            }
        }

        // Fallback: camelCase (most legacy fields use this convention)
        return char.ToLowerInvariant(pascalCase[0]) + pascalCase[1..];
    }
}
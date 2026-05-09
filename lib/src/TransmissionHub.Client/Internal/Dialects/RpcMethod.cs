namespace TransmissionHub.Client.Internal.Dialects;

/// <summary>
/// Identifies the Transmission RPC method to invoke.
/// </summary>
/// <remarks>
/// Each dialect (<see cref="RpcRequestDialect"/>, <see cref="JsonRpcDialect"/>) converts
/// this value to the appropriate wire-format string via a <c>switch</c> expression.
/// </remarks>
internal enum RpcMethod
{
    // Torrent actions
    TorrentStart,
    TorrentStartNow,
    TorrentStop,
    TorrentVerify,
    TorrentReannounce,

    // Torrent mutators / accessors
    TorrentSet,
    TorrentGet,
    TorrentAdd,
    TorrentRemove,
    TorrentSetLocation,
    TorrentRenamePath,

    // Queue
    QueueMoveTop,
    QueueMoveUp,
    QueueMoveDown,
    QueueMoveBottom,

    // Session
    SessionGet,
    SessionSet,
    SessionStats,
    SessionClose,

    // Other
    FreeSpace,
    PortTest,
    BlocklistUpdate,
    GroupGet,
    GroupSet,
}
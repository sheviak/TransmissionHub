namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// Request to move torrents in the queue.
/// </summary>
/// <remarks>
/// Used for queue-move-top, queue-move-up, queue-move-down, and queue-move-bottom.
/// </remarks>
public record QueueMoveRequest
{
    /// <summary>
    /// The torrent list to move.
    /// </summary>
    /// <remarks>
    /// Can contain integer IDs or hash strings.
    /// </remarks>
    public required IReadOnlyList<TorrentId> Ids { get; init; }
}
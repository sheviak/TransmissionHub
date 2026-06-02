namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// Request to remove one or more torrents.
/// </summary>
public record TorrentRemoveRequest
{
    /// <summary>
    /// Torrent list, as described in <see cref="TorrentId"/>.
    /// </summary>
    public required IReadOnlyList<TorrentId> Ids { get; init; }

    /// <summary>
    /// Delete local data. (default: false)
    /// </summary>
    public bool DeleteLocalData { get; init; }
}
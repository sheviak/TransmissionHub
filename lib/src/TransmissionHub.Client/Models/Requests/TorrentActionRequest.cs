namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// A request to perform an action on one or more torrents.
/// Actions include: torrent-start, torrent-start-now, torrent-stop, torrent-verify, torrent-reannounce.
/// </summary>
public record TorrentActionRequest
{
    /// <summary>
    /// Specifies which torrents to use.
    /// All torrents are used if the Ids argument is omitted.
    /// </summary>
    /// <remarks>
    /// Can contain integer IDs or hash strings.
    /// </remarks>
    public IReadOnlyList<TorrentId>? Ids { get; init; }
}
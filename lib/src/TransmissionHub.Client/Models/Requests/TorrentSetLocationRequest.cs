namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// Request to change the storage location of torrents.
/// </summary>
public record TorrentSetLocationRequest
{
    /// <summary>
    /// The torrent list to move.
    /// </summary>
    /// <remarks>
    /// Can contain integer IDs or hash strings.
    /// </remarks>
    public required IReadOnlyList<TorrentId> Ids { get; init; }

    /// <summary>
    /// The new directory for the torrent content.
    /// </summary>
    public required string Location { get; init; }

    /// <summary>
    /// If true, move from previous location. Otherwise, search location for files.
    /// </summary>
    public bool Move { get; init; }
}
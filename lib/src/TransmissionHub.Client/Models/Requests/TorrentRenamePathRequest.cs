namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// Request to rename a torrent's path (file or folder).
/// </summary>
public record TorrentRenamePathRequest
{
    /// <summary>
    /// The torrent list (must only be 1 torrent).
    /// </summary>
    public required IReadOnlyList<TorrentId> Ids { get; init; }

    /// <summary>
    /// The path to the file or folder that will be renamed.
    /// </summary>
    public required string Path { get; init; }

    /// <summary>
    /// The file or folder's new name.
    /// </summary>
    public required string Name { get; init; }
}
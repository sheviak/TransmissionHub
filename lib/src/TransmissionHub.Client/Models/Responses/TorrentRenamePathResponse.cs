namespace TransmissionHub.Client.Models.Responses;

/// <summary>
/// Response to renaming a torrent's path.
/// </summary>
public record TorrentRenamePathResponse
{
    /// <summary>
    /// The path that was renamed.
    /// </summary>
    public string? Path { get; init; }

    /// <summary>
    /// The new name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// The torrent ID integer.
    /// </summary>
    public int? Id { get; init; }
}
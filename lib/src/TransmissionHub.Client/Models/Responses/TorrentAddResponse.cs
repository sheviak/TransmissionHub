namespace TransmissionHub.Client.Models.Responses;

/// <summary>
/// Response returned upon adding a torrent.
/// </summary>
public record TorrentAddResponse
{
    /// <summary>
    /// Populated if the torrent was successfully added.
    /// </summary>
    public TorrentAddedInfo? TorrentAdded { get; init; }

    /// <summary>
    /// Populated if a duplicate torrent was detected.
    /// </summary>
    public TorrentAddedInfo? TorrentDuplicate { get; init; }
}

/// <summary>
/// Minimal torrent info returned by torrent-add.
/// </summary>
public record TorrentAddedInfo
{
    /// <summary>
    /// The torrent's numeric identifier.
    /// </summary>
    public int? Id { get; init; }

    /// <summary>
    /// The torrent's display name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// The torrent's info hash string.
    /// </summary>
    public string? HashString { get; init; }
}
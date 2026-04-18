using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using TransmissionHub.Client.Internal.Converters;

namespace TransmissionHub.Client.Models;

/// <summary>
/// Represents an identifier for a torrent.
/// </summary>
/// <remarks>
/// Can be an integer ID, a hash string, or special literals.
/// </remarks>
[JsonConverter(typeof(TorrentIdConverter))]
[SuppressMessage("ReSharper", "ArrangeMethodOrOperatorBody")]
public readonly struct TorrentId
{
    /// <summary>
    /// Gets the integer torrent ID.
    /// </summary>
    /// <remarks>
    /// Null if the ID is a hash string.
    /// </remarks>
    public int? Id { get; }

    /// <summary>
    /// Gets the torrent hash string or special literal.
    /// </summary>
    /// <remarks>
    /// Null if the ID is an integer.
    /// </remarks>
    public string? HashString { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TorrentId"/> struct with an integer ID.
    /// </summary>
    /// <param name="id">The integer torrent ID.</param>
    public TorrentId(int id)
    {
        Id = id;
        HashString = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TorrentId"/> struct with a hash string.
    /// </summary>
    /// <param name="hashString">The torrent hash string.</param>
    public TorrentId(string hashString)
    {
        Id = null;
        HashString = hashString;
    }

    /// <summary>
    /// Special tag referring to recently-active torrents in older RPC specifications.
    /// </summary>
    /// <remarks>
    /// In RPC versions below 18, it is 'recently-active'.
    /// </remarks>
    public static TorrentId RecentlyActiveV1 => new("recently-active");

    /// <summary>
    /// Special tag referring to recently-active torrents in newer RPC specifications (v18+).
    /// </summary>
    /// <remarks>
    /// From version 18, it is 'recently_active'.
    /// </remarks>
    public static TorrentId RecentlyActiveV2 => new("recently_active");

    /// <summary>
    /// Implicitly converts an integer to a <see cref="TorrentId"/>.
    /// </summary>
    public static implicit operator TorrentId(int id) => new(id);

    /// <summary>
    /// Implicitly converts a string to a <see cref="TorrentId"/>.
    /// </summary>
    public static implicit operator TorrentId(string hashString) => new(hashString);

    /// <summary>
    /// Returns a string representation of the ID.
    /// </summary>
    public override string ToString()
    {
        if (Id.HasValue)
        {
            return Id.Value.ToString();
        }

        return HashString ?? string.Empty;
    }
}
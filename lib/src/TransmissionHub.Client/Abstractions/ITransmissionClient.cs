using TransmissionHub.Client.Models.Requests;
using TransmissionHub.Client.Models.Responses;

namespace TransmissionHub.Client.Abstractions;

/// <summary>
/// Defines a unified client for interacting with the Transmission RPC API.
/// </summary>
/// <remarks>
/// Supports Transmission RPC versions 16, 17, and 18.
/// All methods return a <see cref="Result"/> or <see cref="Result{T}"/> to represent success or failure explicitly.
/// </remarks>
public interface ITransmissionClient
{
    // -------------------------------------------------------------------------
    // Torrent Actions
    // -------------------------------------------------------------------------

    /// <summary>
    /// Starts one or more torrents.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying which torrents to start.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> TorrentStartAsync(TorrentActionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts one or more torrents immediately, bypassing the queue.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying which torrents to start.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> TorrentStartNowAsync(TorrentActionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops one or more torrents.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying which torrents to stop.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> TorrentStopAsync(TorrentActionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies the data of one or more torrents.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying which torrents to verify.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> TorrentVerifyAsync(TorrentActionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reannounces one or more torrents to their trackers.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying which torrents to reannounce.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> TorrentReannounceAsync(TorrentActionRequest request, CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Torrent Mutators
    // -------------------------------------------------------------------------

    /// <summary>
    /// Sets properties on one or more torrents.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request containing properties to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> TorrentSetAsync(TorrentSetRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about one or more torrents.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying which torrents and fields to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result<TorrentGetResponse>> TorrentGetAsync(TorrentGetRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new torrent.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request containing the torrent to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result<TorrentAddResponse>> TorrentAddAsync(TorrentAddRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes one or more torrents.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying which torrents to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> TorrentRemoveAsync(TorrentRemoveRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moves one or more torrents to a new location.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying the target location.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> TorrentSetLocationAsync(TorrentSetLocationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Renames a file or directory within a torrent.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 15.
    /// </remarks>
    /// <param name="request">The request specifying the path and new name.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result<TorrentRenamePathResponse>> TorrentRenamePathAsync(TorrentRenamePathRequest request, CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Queue
    // -------------------------------------------------------------------------

    /// <summary>
    /// Moves one or more torrents to the top of the queue.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying which torrents to move.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> QueueMoveTopAsync(QueueMoveRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moves one or more torrents up in the queue.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying which torrents to move.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> QueueMoveUpAsync(QueueMoveRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moves one or more torrents down in the queue.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying which torrents to move.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> QueueMoveDownAsync(QueueMoveRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moves one or more torrents to the bottom of the queue.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request specifying which torrents to move.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> QueueMoveBottomAsync(QueueMoveRequest request, CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Session
    // -------------------------------------------------------------------------

    /// <summary>
    /// Gets session-level properties.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request optionally specifying which fields to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result<SessionGetResponse>> SessionGetAsync(SessionGetRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets session-level properties.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The request containing properties to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> SessionSetAsync(SessionSetRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets session statistics.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result<SessionStatsResponse>> SessionStatsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Terminates the Transmission daemon session.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> SessionCloseAsync(CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Other
    // -------------------------------------------------------------------------

    /// <summary>
    /// Gets the amount of free space in a given directory.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 15.
    /// </remarks>
    /// <param name="request">The request specifying the directory path.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result<FreeSpaceResponse>> FreeSpaceAsync(FreeSpaceRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tests whether the external port is open.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The port test request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result<PortTestResponse>> PortTestAsync(PortTestRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the blocklist and returns the number of rules loaded.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 16.
    /// </remarks>
    /// <param name="request">The blocklist update request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result<BlocklistUpdateResponse>> BlocklistUpdateAsync(BlocklistUpdateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about bandwidth groups.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 17.
    /// </remarks>
    /// <param name="request">The request optionally specifying which groups to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result<GroupGetResponse>> GroupGetAsync(GroupGetRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets properties on a bandwidth group.
    /// </summary>
    /// <remarks>
    /// Available since RPC version 17.
    /// </remarks>
    /// <param name="request">The request containing group properties to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task<Result> GroupSetAsync(GroupSetRequest request, CancellationToken cancellationToken = default);
}
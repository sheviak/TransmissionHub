namespace TransmissionHub.Client.Models.Responses;

/// <summary>
/// Response to the blocklist update request.
/// </summary>
/// <remarks>
/// Contains the number of rules in the updated blocklist.
/// </remarks>
public record BlocklistUpdateResponse
{
    /// <summary>
    /// Number of rules in the newly updated blocklist.
    /// </summary>
    public int BlocklistSize { get; init; }
}
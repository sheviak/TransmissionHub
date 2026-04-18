namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// Request to get information about bandwidth groups.
/// </summary>
/// <remarks>
/// Added in RPC version 17.
/// </remarks>
public record GroupGetRequest
{
    /// <summary>
    /// A list of strings naming the bandwidth group.
    /// </summary>
    /// <remarks>
    /// If omitted, all bandwidth groups are used.
    /// </remarks>
    public IReadOnlyList<string>? Group { get; init; }
}
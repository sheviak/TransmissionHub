namespace TransmissionHub.Client.Models.Requests;

/// <summary>
/// Tests how much free space is available in a client-specified folder.
/// </summary>
public record FreeSpaceRequest
{
    /// <summary>
    /// The directory path to query.
    /// </summary>
    public string? Path { get; init; }
}
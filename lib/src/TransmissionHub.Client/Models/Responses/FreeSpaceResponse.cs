namespace TransmissionHub.Client.Models.Responses;

/// <summary>
/// Response describing the free space in a specific folder.
/// </summary>
public record FreeSpaceResponse
{
    /// <summary>
    /// The directory path queried.
    /// </summary>
    public string? Path { get; init; }

    /// <summary>
    /// The size, in bytes, of the free space in the directory.
    /// </summary>
    public long? SizeBytes { get; init; }

    /// <summary>
    /// The total capacity, in bytes, of the directory.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    public long? TotalSize { get; init; }
}
using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Internal.Validation.RequestValidators;

/// <inheritdoc />
internal class TorrentRenamePathRequestValidator : IValidatableRequest<TorrentRenamePathRequest>
{
    /// <inheritdoc />
    public Result Validate(TorrentRenamePathRequest request)
    {
        if (request.Ids.Count != 1)
        {
            return Result.Fail("Ids list must contain exactly one torrent ID.");
        }

        if (string.IsNullOrWhiteSpace(request.Path))
        {
            return Result.Fail("Path cannot be null or whitespace.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Result.Fail("Name cannot be null or whitespace.");
        }

        return Result.Ok();
    }
}
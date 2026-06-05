using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Internal.Validation.RequestValidators;

/// <inheritdoc />
internal class TorrentRemoveRequestValidator : IValidatableRequest<TorrentRemoveRequest>
{
    /// <inheritdoc />
    public Result Validate(TorrentRemoveRequest request)
    {
        if (request.Ids.Count == 0)
        {
            return Result.Fail("Ids list cannot be empty.");
        }

        return Result.Ok();
    }
}
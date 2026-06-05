using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Internal.Validation.RequestValidators;

/// <inheritdoc />
internal class TorrentGetRequestValidator : IValidatableRequest<TorrentGetRequest>
{
    /// <inheritdoc />
    public Result Validate(TorrentGetRequest request)
    {
        if (request.Fields.Count == 0)
        {
            return Result.Fail("Fields list cannot be empty.");
        }

        return Result.Ok();
    }
}
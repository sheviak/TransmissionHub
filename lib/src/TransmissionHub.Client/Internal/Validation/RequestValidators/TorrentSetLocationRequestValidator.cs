using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Internal.Validation.RequestValidators;

/// <inheritdoc />
internal class TorrentSetLocationRequestValidator : IValidatableRequest<TorrentSetLocationRequest>
{
    /// <inheritdoc />
    public Result Validate(TorrentSetLocationRequest request)
    {
        if (request.Ids.Count == 0)
        {
            return Result.Fail("Ids list cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(request.Location))
        {
            return Result.Fail("Location cannot be null or whitespace.");
        }

        return Result.Ok();
    }
}
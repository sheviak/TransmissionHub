using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Internal.Validation.RequestValidators;

/// <inheritdoc />
internal sealed class FreeSpaceRequestValidator : IValidatableRequest<FreeSpaceRequest>
{
    /// <inheritdoc />
    public Result Validate(FreeSpaceRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Path))
        {
            return Result.Fail("Path cannot be null or whitespace.");
        }

        return Result.Ok();
    }
}
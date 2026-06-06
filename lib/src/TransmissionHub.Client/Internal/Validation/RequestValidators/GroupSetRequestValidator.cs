using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Internal.Validation.RequestValidators;

/// <inheritdoc />
internal sealed class GroupSetRequestValidator : IValidatableRequest<GroupSetRequest>
{
    /// <inheritdoc />
    public Result Validate(GroupSetRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Result.Fail("Name cannot be null or whitespace.");
        }

        if (request is { SpeedLimitDownEnabled: true, SpeedLimitDown: null })
        {
            return Result.Fail("SpeedLimitDown must be provided when SpeedLimitDownEnabled is true.");
        }

        if (request is { SpeedLimitUpEnabled: true, SpeedLimitUp: null })
        {
            return Result.Fail("SpeedLimitUp must be provided when SpeedLimitUpEnabled is true.");
        }

        return Result.Ok();
    }
}
using Microsoft.Extensions.DependencyInjection;
using TransmissionHub.Client.Abstractions;

namespace TransmissionHub.Client.Internal.Validation;

/// <inheritdoc />
internal sealed class ValidatorProvider(IServiceProvider provider) : IValidatorProvider
{
    /// <inheritdoc />
    public Result Validate<TRequest>(TRequest request) where TRequest : class
    {
        var validator = provider.GetService<IValidatableRequest<TRequest>>();

        return validator?.Validate(request) ?? Result.Ok();
    }
}
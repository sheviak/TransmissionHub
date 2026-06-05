using TransmissionHub.Client.Abstractions;

namespace TransmissionHub.Client.Internal.Validation;

/// <summary>
/// Defines a provider that finds and executes validators for request models.
/// </summary>
internal interface IValidatorProvider
{
    /// <summary>
    /// Validates a given request object if a corresponding validator is registered.
    /// </summary>
    /// <param name="request">The request object to validate.</param>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <returns>
    /// A <see cref="Result"/> indicating the outcome of the validation.
    /// If no validator is found for the request type, returns <see cref="Result.Ok()"/>.
    /// </returns>
    public Result Validate<TRequest>(TRequest request) where TRequest : class;
}
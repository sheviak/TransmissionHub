using TransmissionHub.Client.Abstractions;

namespace TransmissionHub.Client.Internal.Validation;

/// <summary>
/// Defines a validator for a specific request type.
/// </summary>
/// <typeparam name="TRequest">The type of the request to validate.</typeparam>
internal interface IValidatableRequest<in TRequest> where TRequest : class
{
    /// <summary>
    /// Validates the specified request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>A <see cref="Result"/> indicating the outcome of the validation.</returns>
    public Result Validate(TRequest request);
}
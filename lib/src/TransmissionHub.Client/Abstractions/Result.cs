using System.Diagnostics.CodeAnalysis;

namespace TransmissionHub.Client.Abstractions;

/// <summary>
/// Represents the outcome of an operation that returns no value.
/// </summary>
/// <remarks>
/// Use <see cref="Result.Ok()"/> to indicate success and <see cref="Result.Fail(TransmissionHub.Client.Abstractions.Error)"/> to indicate failure.
/// </remarks>
public readonly struct Result
{
    private readonly Error? _error;

    private Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        _error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error, or <see langword="null"/> if the operation succeeded.
    /// </summary>
    public Error? Error => _error;

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static Result Ok() => new(isSuccess: true, error: null);

    /// <summary>
    /// Creates a successful result with a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value produced by the operation.</param>
    public static Result<T> Ok<T>(T value) => Result<T>.Ok(value);

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    public static Result Fail(Error error) => new(isSuccess: false, error: error);

    /// <summary>
    /// Creates a failed result with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="code">An optional error code.</param>
    public static Result Fail(string message, int? code = null) =>
        Fail(new Error(message, code));

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> with the given error.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="error">The error that caused the failure.</param>
    public static Result<T> Fail<T>(Error error) => Result<T>.Fail(error);

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> with the given error message.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="message">The error message.</param>
    /// <param name="code">An optional error code.</param>
    public static Result<T> Fail<T>(string message, int? code = null) =>
        Result<T>.Fail(new Error(message, code));

    /// <inheritdoc />
    public override string ToString() => IsSuccess ? "Success" : $"Failure: {_error}";
}

/// <summary>
/// Represents the outcome of an operation that returns a value of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the value produced on success.</typeparam>
/// <remarks>
/// Use <see cref="Result.Ok{T}"/> to indicate success and <see cref="Result.Fail{T}(TransmissionHub.Client.Abstractions.Error)"/> to indicate failure.
/// </remarks>
public readonly struct Result<T>
{
    private readonly T? _value;
    private readonly Error? _error;

    private Result(bool isSuccess, T? value, Error? error)
    {
        IsSuccess = isSuccess;
        _value = value;
        _error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the value produced by the operation.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the result is a failure.</exception>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException(
            $"Cannot access Value on a failed result. Error: {_error}");

    /// <summary>
    /// Gets the error, or <see langword="null"/> if the operation succeeded.
    /// </summary>
    public Error? Error => _error;

    /// <summary>
    /// Creates a successful result with a value.
    /// </summary>
    /// <param name="value">The value produced by the operation.</param>
    internal static Result<T> Ok(T value) => new(isSuccess: true, value: value, error: null);

    /// <summary>
    /// Creates a failed result with the given error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    internal static Result<T> Fail(Error error) => new(isSuccess: false, value: default, error: error);

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="T"/> to a successful <see cref="Result{T}"/>.
    /// </summary>
    public static implicit operator Result<T>(T value) => Ok(value);

    /// <summary>
    /// Implicitly converts an <see cref="Abstractions.Error"/> to a failed <see cref="Result{T}"/>.
    /// </summary>
    public static implicit operator Result<T>(Error error) => Fail(error);

    /// <inheritdoc />
    public override string ToString() => IsSuccess
        ? $"Success: {_value}"
        : $"Failure: {_error}";
}
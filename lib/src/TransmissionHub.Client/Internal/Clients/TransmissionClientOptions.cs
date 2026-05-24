using System.ComponentModel.DataAnnotations;

namespace TransmissionHub.Client.Internal.Clients;

/// <summary>
/// Configures Transmission client integration settings.
/// </summary>
internal sealed class TransmissionClientOptions : IValidatableObject
{
    /// <summary>
    /// List of supported Transmission RPC versions.
    /// </summary>
    private static readonly int[] SupportedVersions = [16, 17, 18];

    /// <summary>
    /// Gets or sets the Transmission RPC endpoint URL.
    /// </summary>
    [Required]
    public Uri? Url { get; init; }

    /// <summary>
    /// Gets or sets a default download directory used by consumers.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string DownloadDirectory { get; init; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether basic authentication should be used.
    /// </summary>
    /// <remarks>
    /// The default value is <c>True</c>.
    /// </remarks>
    public bool RequiresAuthentication { get; init; } = true;

    /// <summary>
    /// Gets or sets the basic authentication login.
    /// </summary>
    public string? Login { get; init; }

    /// <summary>
    /// Gets or sets the basic authentication password.
    /// </summary>
    public string? Password { get; init; }

    /// <summary>
    /// Gets or sets the target Transmission RPC version.
    /// </summary>
    [Required]
    public int RpcVersion { get; init; }

    /// <summary>
    /// Gets or sets the HTTP timeout in seconds.
    /// </summary>
    /// <remarks>
    /// The default value is 10.
    /// </remarks>
    [Range(1, 300)]
    public int TimeoutSeconds { get; init; } = 10;

    /// <summary>
    /// Gets or sets the maximum number of retries for a Session ID (409 Conflict).
    /// </summary>
    [Range(1, 5)]
    public int MaxSessionRetries { get; init; } = 1;

    /// <summary>
    /// Gets or sets a value indicating whether to log RPC requests.
    /// </summary>
    /// <remarks>
    /// The default value is <c>False</c>.
    /// </remarks>
    public bool LogRequests { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to log RPC responses.
    /// </summary>
    /// <remarks>
    /// The default value is <c>False</c>.
    /// </remarks>
    public bool LogResponses { get; init; }

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Url is not null && !Url.IsAbsoluteUri)
        {
            yield return new ValidationResult(
                "Transmission URL must be absolute.",
                [nameof(Url)]);
        }

        if (!SupportedVersions.Contains(RpcVersion))
        {
            yield return new ValidationResult(
                $"Transmission RPC version must be one of: {string.Join(", ", SupportedVersions)}.",
                [nameof(RpcVersion)]);
        }

        if (!RequiresAuthentication)
        {
            yield break;
        }

        if (string.IsNullOrWhiteSpace(Login))
        {
            yield return new ValidationResult(
                "Login is required when authentication is enabled.",
                [nameof(Login)]);
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            yield return new ValidationResult(
                "Password is required when authentication is enabled.",
                [nameof(Password)]
            );
        }
    }
}
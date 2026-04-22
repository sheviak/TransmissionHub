namespace TransmissionHub.Client.Models.Enums;

/// <summary>
/// Encryption preference for the session.
/// </summary>
/// <remarks>
/// Used in session-get and session-set.
/// </remarks>
public enum EncryptionMode
{
    /// <summary>
    /// Requires encrypted connections.
    /// </summary>
    /// <remarks>
    /// Corresponds to 'required'.
    /// </remarks>
    Required = 0,

    /// <summary>
    /// Prefers encrypted connections.
    /// </summary>
    /// <remarks>
    /// Corresponds to 'preferred'.
    /// </remarks>
    Preferred = 1,

    /// <summary>
    /// Does not require encryption.
    /// </summary>
    /// <remarks>
    /// Deprecated in RPC version 17. Use <see cref="Allowed"/>.
    /// </remarks>
    Tolerated = 2,

    /// <summary>
    /// Connections can be encrypted or unencrypted.
    /// </summary>
    /// <remarks>
    /// Added in RPC version 17.
    /// </remarks>
    Allowed = 2,
}
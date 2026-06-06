using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Internal.Validation.RequestValidators;

/// <inheritdoc />
internal sealed class TorrentAddRequestValidator : IValidatableRequest<TorrentAddRequest>
{
    /// <inheritdoc />
    public Result Validate(TorrentAddRequest request)
    {
        var hasFilename = !string.IsNullOrWhiteSpace(request.Filename);
        var hasMetainfo = !string.IsNullOrWhiteSpace(request.Metainfo);

        if (hasFilename == hasMetainfo)
        {
            return Result.Fail("Exactly one of Filename or Metainfo must be provided.");
        }

        return Result.Ok();
    }
}
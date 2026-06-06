using TransmissionHub.Client.Internal.Validation.RequestValidators;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Tests.Internal.Validation.RequestValidators;

public class TorrentAddRequestValidatorTests
{
    private readonly TorrentAddRequestValidator _validator = new();

    [Test]
    public async Task Validate_FilenameProvided_ReturnsOk()
    {
        // Arrange
        var request = new TorrentAddRequest
        {
            Filename = "ubuntu.torrent"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Validate_MetainfoProvided_ReturnsOk()
    {
        // Arrange
        var request = new TorrentAddRequest
        {
            Metainfo = "base64encodeddata"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Validate_BothProvided_ReturnsFail()
    {
        // Arrange
        var request = new TorrentAddRequest
        {
            Filename = "ubuntu.torrent",
            Metainfo = "base64encodeddata"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }

    [Test]
    public async Task Validate_NeitherProvided_ReturnsFail()
    {
        // Arrange
        var request = new TorrentAddRequest();

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }
}
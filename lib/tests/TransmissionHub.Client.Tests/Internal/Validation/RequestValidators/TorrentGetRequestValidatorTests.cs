using TransmissionHub.Client.Internal.Validation.RequestValidators;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Tests.Internal.Validation.RequestValidators;

public class TorrentGetRequestValidatorTests
{
    private readonly TorrentGetRequestValidator _validator = new();

    [Test]
    public async Task Validate_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new TorrentGetRequest { Fields = [TorrentGetRequest.TorrentFields.Id] };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Validate_FieldsEmpty_ReturnsFail()
    {
        // Arrange
        var request = new TorrentGetRequest { Fields = [] };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }

    [Test]
    public async Task Validate_FieldsNull_ReturnsFail()
    {
        // Arrange
        var request = new TorrentGetRequest { Fields = null! };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }
}
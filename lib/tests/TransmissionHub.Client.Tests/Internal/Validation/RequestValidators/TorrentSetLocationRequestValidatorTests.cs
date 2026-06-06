using TransmissionHub.Client.Internal.Validation.RequestValidators;
using TransmissionHub.Client.Models;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Tests.Internal.Validation.RequestValidators;

public class TorrentSetLocationRequestValidatorTests
{
    private readonly TorrentSetLocationRequestValidator _validator = new();

    [Test]
    public async Task Validate_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new TorrentSetLocationRequest
        {
            Ids = [new TorrentId(1)],
            Location = "/new/location"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Validate_EmptyIds_ReturnsFail()
    {
        // Arrange
        var request = new TorrentSetLocationRequest
        {
            Ids = [],
            Location = "/new/location"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments(null)]
    public async Task Validate_InvalidLocation_ReturnsFail(string? location)
    {
        // Arrange
        var request = new TorrentSetLocationRequest
        {
            Ids = [new TorrentId(1)],
            Location = location!
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }
}
using TransmissionHub.Client.Internal.Validation.RequestValidators;
using TransmissionHub.Client.Models;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Tests.Internal.Validation.RequestValidators;

public class QueueMoveRequestValidatorTests
{
    private readonly QueueMoveRequestValidator _validator = new();

    [Test]
    public async Task Validate_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new QueueMoveRequest { Ids = [new TorrentId(1)] };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Validate_EmptyIds_ReturnsFail()
    {
        // Arrange
        var request = new QueueMoveRequest { Ids = [] };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }
}
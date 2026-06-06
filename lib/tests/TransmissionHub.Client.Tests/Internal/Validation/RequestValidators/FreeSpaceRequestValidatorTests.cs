using TransmissionHub.Client.Internal.Validation.RequestValidators;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Tests.Internal.Validation.RequestValidators;

public class FreeSpaceRequestValidatorTests
{
    private readonly FreeSpaceRequestValidator _validator = new();

    [Test]
    public async Task Validate_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new FreeSpaceRequest { Path = "/downloads" };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments(null)]
    public async Task Validate_InvalidPath_ReturnsFail(string? path)
    {
        // Arrange
        var request = new FreeSpaceRequest { Path = path! };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }
}
using TransmissionHub.Client.Internal.Validation.RequestValidators;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Tests.Internal.Validation.RequestValidators;

public class GroupSetRequestValidatorTests
{
    private readonly GroupSetRequestValidator _validator = new();

    [Test]
    public async Task Validate_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new GroupSetRequest
        {
            Name = "group",
            SpeedLimitDownEnabled = true,
            SpeedLimitDown = 100,
            SpeedLimitUpEnabled = true,
            SpeedLimitUp = 100,
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments(null)]
    public async Task Validate_InvalidName_ReturnsFail(string? name)
    {
        // Arrange
        var request = new GroupSetRequest
        {
            Name = name!,
            SpeedLimitDownEnabled = false,
            SpeedLimitUpEnabled = false
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }

    [Test]
    public async Task Validate_SpeedLimitDownEnabled_ButValueIsNull_ReturnsFail()
    {
        // Arrange
        var request = new GroupSetRequest
        {
            Name = "My-Group",
            SpeedLimitDownEnabled = true,
            SpeedLimitDown = null, // Invalid state
            SpeedLimitUpEnabled = false,
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }

    [Test]
    public async Task Validate_SpeedLimitUpEnabled_ButValueIsNull_ReturnsFail()
    {
        // Arrange
        var request = new GroupSetRequest
        {
            Name = "My-Group",
            SpeedLimitUpEnabled = true,
            SpeedLimitUp = null, // Invalid state
            SpeedLimitDownEnabled = false,
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }
}
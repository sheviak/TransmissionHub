using Microsoft.Extensions.DependencyInjection;
using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Internal.Validation;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Tests.Internal.Validation;

public class ValidatorProviderTests
{
    private readonly IValidatorProvider _validatorProvider = new ServiceCollection()
        .AddTransmissionValidation()
        .BuildServiceProvider()
        .GetRequiredService<IValidatorProvider>();

    [Test]
    public async Task Validate_RequestWithoutValidator_ReturnsOk()
    {
        // Arrange
        var request = new SessionGetRequest(); // This request has no validator

        // Act
        var result = _validatorProvider.Validate(request);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Validate_ValidRequestWithValidator_ReturnsOk()
    {
        // Arrange
        var request = new TorrentAddRequest { Filename = "test.torrent" };

        // Act
        var result = _validatorProvider.Validate(request);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Validate_InvalidRequestWithValidator_ReturnsFail()
    {
        // Arrange
        var request = new TorrentAddRequest(); // Invalid, missing filename/metainfo

        // Act
        var result = _validatorProvider.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }
}
using TransmissionHub.Client.Internal.Validation.RequestValidators;
using TransmissionHub.Client.Models;
using TransmissionHub.Client.Models.Requests;

namespace TransmissionHub.Client.Tests.Internal.Validation.RequestValidators;

public class TorrentRenamePathRequestValidatorTests
{
    private readonly TorrentRenamePathRequestValidator _validator = new();

    [Test]
    public async Task Validate_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new TorrentRenamePathRequest
        {
            Ids = [new TorrentId(1)],
            Path = "old/path",
            Name = "new_name"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Validate_IdsCountZero_ReturnsFail()
    {
        // Arrange
        var request = new TorrentRenamePathRequest
        {
            Ids = [],
            Path = "old/path",
            Name = "new_name"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }

    [Test]
    public async Task Validate_IdsCountGreaterThanOne_ReturnsFail()
    {
        // Arrange
        var request = new TorrentRenamePathRequest
        {
            Ids = [new TorrentId(1), new TorrentId(2)],
            Path = "old/path",
            Name = "new_name"
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
    public async Task Validate_InvalidPath_ReturnsFail(string path)
    {
        // Arrange
        var request = new TorrentRenamePathRequest
        {
            Ids = [new TorrentId(1)],
            Path = path,
            Name = "new_name"
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
    public async Task Validate_InvalidName_ReturnsFail(string name)
    {
        // Arrange
        var request = new TorrentRenamePathRequest
        {
            Ids = [new TorrentId(1)],
            Path = "old/path",
            Name = name
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsNotEmpty();
    }
}
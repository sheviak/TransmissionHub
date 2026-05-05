using TransmissionHub.Client.Abstractions;

namespace TransmissionHub.Client.Tests.Abstractions;

public class ErrorTests
{
    [Test]
    [Arguments("Something went wrong")]
    [Arguments("Not found")]
    public async Task Constructor_MessageOnly_SetsMessageAndNullCode(string message)
    {
        // Arrange & Act
        var error = new Error(message);

        // Assert
        await Assert.That(error.Message).IsEqualTo(message);
        await Assert.That(error.Code).IsNull();
    }

    [Test]
    [Arguments("RPC error", 409)]
    [Arguments("HTTP error", 500)]
    public async Task Constructor_WithCode_SetsBothProperties(string message, int code)
    {
        // Arrange & Act
        var error = new Error(message, code);

        // Assert
        await Assert.That(error.Message).IsEqualTo(message);
        await Assert.That(error.Code).IsEqualTo(code);
    }

    [Test]
    public async Task ToString_WithCode_IncludesCodeAndMessage()
    {
        // Arrange
        var error = new Error("Not found", 404);

        // Act
        var result = error.ToString();

        // Assert
        await Assert.That(result).IsEqualTo("[404] Not found");
    }

    [Test]
    public async Task ToString_WithoutCode_ReturnsMessageOnly()
    {
        // Arrange
        var error = new Error("Something failed");

        // Act
        var result = error.ToString();

        // Assert
        await Assert.That(result).IsEqualTo("Something failed");
    }
}
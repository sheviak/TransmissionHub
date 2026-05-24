using TransmissionHub.Client.Abstractions;

namespace TransmissionHub.Client.Tests.Abstractions;

public class ResultTests
{
    [Test]
    public async Task Ok_IsSuccess_IsTrue()
    {
        // Arrange & Act
        var result = Result.Ok();

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Ok_IsFailure_IsFalse()
    {
        // Arrange & Act
        var result = Result.Ok();

        // Assert
        await Assert.That(result.IsFailure).IsFalse();
    }

    [Test]
    public async Task Ok_Error_IsNull()
    {
        // Arrange & Act
        var result = Result.Ok();

        // Assert
        await Assert.That(result.Error).IsNull();
    }

    [Test]
    public async Task Fail_WithError_IsFailure_IsTrue()
    {
        // Arrange & Act
        var result = Result.Fail(new Error("error"));

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
    }

    [Test]
    public async Task Fail_WithError_IsSuccess_IsFalse()
    {
        // Arrange & Act
        var result = Result.Fail(new Error("error"));

        // Assert
        await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    public async Task Fail_WithError_Error_IsSet()
    {
        // Arrange
        var error = new Error("something failed", 42);

        // Act
        var result = Result.Fail(error);

        // Assert
        await Assert.That(result.Error).IsNotNull();
        await Assert.That(result.Error!.Value.Message).IsEqualTo("something failed");
        await Assert.That(result.Error!.Value.Code).IsEqualTo(42);
    }

    [Test]
    [Arguments("connection refused", null)]
    [Arguments("timeout", 408)]
    public async Task Fail_WithMessage_SetsError(string message, int? code)
    {
        // Arrange & Act
        var result = Result.Fail(message, code);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsEqualTo(message);
        await Assert.That(result.Error!.Value.Code).IsEqualTo(code);
    }

    [Test]
    public async Task Ok_ToString_ReturnsSuccess()
    {
        // Arrange & Act
        var result = Result.Ok();

        // Assert
        await Assert.That(result.ToString()).IsEqualTo("Success");
    }

    [Test]
    public async Task Fail_ToString_ReturnsFailureWithError()
    {
        // Arrange & Act
        var result = Result.Fail("oops");

        // Assert
        await Assert.That(result.ToString()).IsEqualTo("Failure: oops");
    }
}
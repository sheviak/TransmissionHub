using TransmissionHub.Client.Abstractions;

namespace TransmissionHub.Client.Tests.Abstractions;

public class ResultOfTTests
{
    [Test]
    public async Task Ok_IsSuccess_IsTrue()
    {
        // Arrange & Act
        var result = Result.Ok(42);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Ok_IsFailure_IsFalse()
    {
        // Arrange & Act
        var result = Result.Ok("hello");

        // Assert
        await Assert.That(result.IsFailure).IsFalse();
    }

    [Test]
    [Arguments(0)]
    [Arguments(42)]
    [Arguments(-1)]
    public async Task Ok_Value_ReturnsProvidedValue(int value)
    {
        // Arrange & Act
        var result = Result.Ok(value);

        // Assert
        await Assert.That(result.Value).IsEqualTo(value);
    }

    [Test]
    public async Task Ok_Error_IsNull()
    {
        // Arrange & Act
        var result = Result.Ok("data");

        // Assert
        await Assert.That(result.Error).IsNull();
    }

    [Test]
    public async Task Fail_IsFailure_IsTrue()
    {
        // Arrange & Act
        var result = Result.Fail<int>(new Error("failed"));

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
    }

    [Test]
    public async Task Fail_IsSuccess_IsFalse()
    {
        // Arrange & Act
        var result = Result.Fail<string>("error");

        // Assert
        await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    public async Task Fail_Error_IsSet()
    {
        // Arrange
        var error = new Error("rpc error", 500);

        // Act
        var result = Result.Fail<string>(error);

        // Assert
        await Assert.That(result.Error!.Value.Message).IsEqualTo("rpc error");
        await Assert.That(result.Error!.Value.Code).IsEqualTo(500);
    }

    [Test]
    public async Task Value_OnFailedResult_ThrowsInvalidOperationException()
    {
        // Arrange
        var result = Result.Fail<int>("something went wrong");

        // Act
        var action = () => result.Value;

        // Assert
        await Assert.That(action).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task ImplicitConversion_FromValue_CreatesSuccessResult()
    {
        // Arrange & Act
        Result<int> result = 99;

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value).IsEqualTo(99);
    }

    [Test]
    public async Task ImplicitConversion_FromError_CreatesFailureResult()
    {
        // Arrange & Act
        Result<string> result = new Error("implicit fail", 503);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error!.Value.Message).IsEqualTo("implicit fail");
    }

    [Test]
    public async Task Ok_ToString_ReturnsSuccessWithValue()
    {
        // Arrange & Act
        var result = Result.Ok(7);

        // Assert
        await Assert.That(result.ToString()).IsEqualTo("Success: 7");
    }

    [Test]
    public async Task Fail_ToString_ReturnsFailureWithError()
    {
        // Arrange & Act
        var result = Result.Fail<int>("bad request", 400);

        // Assert
        await Assert.That(result.ToString()).IsEqualTo("Failure: [400] bad request");
    }
}
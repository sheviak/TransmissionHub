namespace TransmissionHub.Client.Tests;

public class StubTests
{
    [Test]
    public async Task OnePlusOne_Two_Success()
    {
        // Arrange
        const int number = 1;
        const int expected = 2;

        // Act
        var result = number + number;

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }
}
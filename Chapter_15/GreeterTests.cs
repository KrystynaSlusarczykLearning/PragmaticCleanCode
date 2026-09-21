using Moq;
using NUnit.Framework;

[TestFixture]
public class GreeterTests
{
    [Test]
    public void Greet_ShallShow_HelloName_Message()
    {
        var userCommunicationMock = new Mock<IUserCommunication>();

        var greeter = new Greeter(userCommunicationMock.Object);

        greeter.Greet("Ali");

        // Validates if a method has been called on a mock.
        userCommunicationMock.Verify(
            mock => mock.ShowMessage("Hello, Ali!"));
    }
}

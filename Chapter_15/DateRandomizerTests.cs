using Moq;
using NUnit.Framework;

[TestFixture]
public class DateRandomizerTests
{
    [Test]
    public void GenerateFor_ShallGenerateDateFromGivenYear_WithRandomDay()
    {
        var year = 2024;

        var randomizedDaysOffset = 10;
        var randomMock = new Mock<IRandom>();
        randomMock.Setup(mock => mock.Next(It.IsAny<int>())).Returns(randomizedDaysOffset);

        var dateRandomizer = new DateRandomizer(randomMock.Object);

        var result = dateRandomizer.GenerateFor(year);

        Assert.That(result.Year, Is.EqualTo(year));
        Assert.That(result.Day, Is.EqualTo(randomizedDaysOffset + 1));

        //those asserts are not relevant and should be removed
        Assert.That(result.Hour, Is.EqualTo(0));
        Assert.That(result.Minute, Is.EqualTo(0));
        Assert.That(result.Second, Is.EqualTo(0));
    }

    //in this case, a loop in test may be acceptable
    //(could be replaced with a collection comparison though)
    [Test]
    public void AcceptableLoopInTests()
    {
        var input = new int[] { 1, -5, 3, -2 };

        var result = input.Where(number => number > 0);

        foreach(var number in result)
        {
            Assert.That(number > 0);
        }
    }
}

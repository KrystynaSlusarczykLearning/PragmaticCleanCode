using NUnit.Framework;

[TestFixture]
class MathTests
{
    [SetUp]
    public void SetUp()
    {
        // Code written here will be run before every test.
    }

    [Test]
    public void Add_ShallGive_8_ForArguments_3_And_5()
    {
        var sum = Math.Add(3, 5);

        Assert.That(sum, Is.EqualTo(8));
    }

    [Test]
    public void Divide_ShallGive_2_WhenDividing_8_By_4()
    {
        var result = Math.Divide(8, 4);

        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void Divide_ShallGive_4_WhenDividing_20_By_5()
    {
        var result = Math.Divide(20, 5);

        Assert.That(result, Is.EqualTo(4));
    }

    // It's better to use test cases
    // instead of copying tests.
    [TestCase(8, 4, 2)]
    [TestCase(20, 5, 4)]
    public void Divide_ShallGiveCorrectResult_ForValidInput(
        int dividend, int divisor, float expectedResult)
    {
        var result = Math.Divide(dividend, divisor);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void Divide_ShallThrowException_WhenDividingBy_0()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => Math.Divide(5, 0));

        Assert.That(
            exception.Message,
            Is.EqualTo("Attempted to divide by zero."));
    }

    // We should have two tests instead of this one,
    // each with a single assertion.
    [Test]
    public void Math_ShallGiveCorrectResults()
    {
        var addResult = Math.Add(3, 5);
        Assert.That(addResult, Is.EqualTo(8));

        var divideResult = Math.Divide(8, 4);
        Assert.That(divideResult, Is.EqualTo(2));
    }
}

using NUnit.Framework;

[TestFixture]
public class ListTests
{
    // Multiple asserts per test may be justified
    // when they are closely related
    [Test]
    public void Reverse_ShallFlipTheCollectionOrder()
    {
        var input = new List<int> { 1, 2, 3 };

        input.Reverse();

        Assert.That(input.Count, Is.EqualTo(3));
        Assert.That(input[0], Is.EqualTo(3));
        Assert.That(input[1], Is.EqualTo(2));
        Assert.That(input[2], Is.EqualTo(1));
    }
}

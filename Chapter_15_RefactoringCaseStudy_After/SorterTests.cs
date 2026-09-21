using NUnit.Framework;

[TestFixture]
public class SorterTests
{
    [TestCase(new[] { 4, 2, 3, 1 }, new[] { 1, 2, 3, 4 })]

    [TestCase(new[] { 3, 4, 2, 1 }, new[] { 1, 2, 3, 4 })]
    public void BubbleSort_ShallProduceListSortedAscendingly_FromUnsortedInput(
        int[] unsortedCollection, int[] expectedSortedCollection)
    {
        List<int> resultList = Sorter.BubbleSort(unsortedCollection.ToList());

        Assert.That(resultList, Is.EqualTo(expectedSortedCollection));
    }

    [Test]
    public void BubbleSort_ShallProduceEmptyList_FromEmptyInput()
    {
        List<int> emptyList = new List<int>();

        List<int> resultList = Sorter.BubbleSort(emptyList);

        Assert.That(resultList.Count, Is.EqualTo(0));
    }

    [Test]
    public void BubbleSort_ShallProduceTheSameList_FromSingleItemInput()
    {
        List<int> singleElementList = new List<int> { 42 };
        List<int> expectedSortedList = new List<int> { 42 };

        List<int> resultList = Sorter.BubbleSort(singleElementList);

        Assert.That(resultList, Is.EqualTo(expectedSortedList));
    }

    [Test]
    public void BubbleSort_ShallProduceTheSameList_FromAlreadySortedInput()
    {
        List<int> sortedList = new List<int> { 1, 2, 3, 4, 5 };
        List<int> expectedSortedList = new List<int> { 1, 2, 3, 4, 5 };

        List<int> resultList = Sorter.BubbleSort(sortedList);

        Assert.That(resultList, Is.EqualTo(expectedSortedList));
    }

    [Test]
    public void BubbleSort_ShallProduceListSortedAscendingly_FromInputWithDuplicates()
    {
        List<int> listWithDuplicates = new List<int> { 3, 5, 2, 2, 5, 1, 3 };
        List<int> expectedSortedList = new List<int> { 1, 2, 2, 3, 3, 5, 5 };

        List<int> resultList = Sorter.BubbleSort(listWithDuplicates);

        Assert.That(resultList, Is.EqualTo(expectedSortedList));
    }

    [Test]
    public void BubbleSort_ShallThrowException_ForNullInput()
    {
        Assert.That(() => Sorter.BubbleSort(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void BubbleSort_ShallNotModifyTheInputList()
    {
        List<int> inputList = new List<int> { 3, 2, 1 };

        var resultList = Sorter.BubbleSort(inputList);

        Assert.That(inputList, Is.EqualTo(new List<int> { 3, 2, 1 }));
    }
}
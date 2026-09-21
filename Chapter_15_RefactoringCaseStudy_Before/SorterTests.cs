using NUnit.Framework;

[TestFixture]
public class SorterTests
{
    [Test]
    public void BubbleSortTest()
    {
        // Arrange
        List<int> unsortedList1 = new List<int> { 4, 2, 3, 1 };
        List<int> expectedSortedList = new List<int> { 1, 2, 3, 4 };

        // Act
        List<int> resultList1 = Sorter.BubbleSort(unsortedList1);

        // Assert
        Assert.That(resultList1, Is.EqualTo(expectedSortedList));

        // Arrange
        List<int> unsortedList2 = new List<int> { 3, 4, 2, 1 };

        // Act
        List<int> resultList2 = Sorter.BubbleSort(unsortedList2);

        // Assert
        Assert.That(resultList2, Is.EqualTo(expectedSortedList));

        // Arrange
        List<int> unsortedList3 = new List<int> { 1, 2, 3, 4 };

        // Act
        List<int> resultList3 = Sorter.BubbleSort(unsortedList3);

        // Assert
        Assert.That(resultList3, Is.EqualTo(expectedSortedList));
    }

    [Test]
    public void EmptyList_ReturnsEmptyList()
    {
        List<int> emptyList = new List<int>();

        List<int> resultList = Sorter.BubbleSort(emptyList);

        Assert.That(resultList.Count, Is.EqualTo(0));
    }

    [Test]
    public void ShallReturnSameList_ForOneItemList()
    {
        // Arrange
        List<int> singleElementList = new List<int> { 42 };
        List<int> expectedSortedList = new List<int> { 42 };

        // Act
        List<int> resultList = Sorter.BubbleSort(singleElementList);

        Assert.That(resultList, Is.EqualTo(expectedSortedList));
    }

    [Test]
    public void BubbleSort_ShallProduceTheSameList_FromAlreadySortedInput()
    {
        // Arrange
        List<int> sortedList = new List<int> { 1, 2, 3, 4, 5 };
        List<int> expectedSortedList = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        List<int> resultList = Sorter.BubbleSort(sortedList);

        // Assert
        Assert.That(resultList, Is.EqualTo(expectedSortedList));
    }

    [Test]
    public void BubbleSort_ListWithDuplicates_SortsCorrectly()
    {
        List<int> listWithDuplicates = new List<int> { 3, 5, 2, 2, 5, 1, 3 };
        List<int> expectedSortedList = new List<int> { 1, 2, 2, 3, 3, 5, 5 };

        List<int> resultList = Sorter.BubbleSort(listWithDuplicates);

        Assert.That(resultList, Is.EqualTo(expectedSortedList));
    }
}
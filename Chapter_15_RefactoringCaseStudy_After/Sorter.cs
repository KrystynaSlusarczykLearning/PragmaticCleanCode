public class Sorter
{
    //https://en.wikipedia.org/wiki/Bubble_sort

    public static List<int> BubbleSort(List<int> inputList)
    {
        ArgumentNullException.ThrowIfNull(inputList);

        List<int> sortedList = new List<int>(inputList);

        int itemsCount = sortedList.Count; 
        for (int i = 0; i < itemsCount - 1; i++)
        {
            for (int j = 0; j < itemsCount - i - 1; j++)
            {
                if (sortedList[j] > sortedList[j + 1])
                {
                    int temp = sortedList[j];
                    sortedList[j] = sortedList[j + 1];
                    sortedList[j + 1] = temp;
                }
            } 
        } 

        return sortedList;
    }
}

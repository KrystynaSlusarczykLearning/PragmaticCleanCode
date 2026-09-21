public class Sorter
{
    //impl of the bubble sort algorithm
    //O(n*n) complexity
    //Bubble sort, sometimes referred to as sinking sort,
    //is a simple sorting algorithm that repeatedly steps through
    //the input list element by element, comparing the current element
    //with the one after it, swapping their values if needed.

    //implemented by Krystyna on 12.12.2024
    //modified on 20.12.2024 to not modify the input list
    public static List<int> BubbleSort(List<int> inputList)
    {
        //check if input is null
        //throw ex if so
        ArgumentNullException.ThrowIfNull(inputList);

        List<int> sortedList = new List<int>(inputList);

        int n = sortedList.Count; //the count of items in thel ist
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (sortedList[j] > sortedList[j + 1])
                {
                    // Swap sortedList[j] and sortedList[j + 1]
                    int temp = sortedList[j];
                    sortedList[j] = sortedList[j + 1];
                    sortedList[j + 1] = temp;
                }
            } //end of inner for loop
        } //end of outer for loop

        return sortedList;
    }

    //old version that modifies the input list

    //public static List<int> BubbleSort(List<int> list)
    //{
    //    int n = list.Count; 
    //    for (int i = 0; i < n - 1; i++)
    //    {
    //        for (int j = 0; j < n - i - 1; j++)
    //        {
    //            if (list[j] > list[j + 1])
    //            {
    //                // Swap list[j] and list[j + 1]
    //                int temp = list[j];
    //                list[j] = list[j + 1];
    //                list[j + 1] = temp;
    //            }
    //        }
    //    }
    //    return list;
    //}
}

using System.Text.Json;
using System.Text.RegularExpressions;

// ### Examples of bad comments below

int iterations = 10;
decimal z = 0;
for(int i = 0; i < iterations; i++) 
{
decimal x = i;
decimal y = x * i;
z += y;
} //end of for loop

// (Useless comment above, because if the formatting was correct,
// there would be no need to state where the loop ends.)

Console.WriteLine("Press any key to close.");
Console.ReadKey();


// Calculates the distance between points in kilometers 
// (Useless comment, because the method name already states the same.)
double CalculateDistanceInKilometers(Point point1, Point point2)
{
    double deltaX = point2.X - point1.X;
    double deltaY = point2.Y - point1.Y;

    return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
}

void SampleMethod_BadComments(string name, string dir, string ex, int id)
{
    var file = dir + "/" + name + "." + ex;
    List<int> numbers = new List<int>();
    if (ex == "txt")
    {
        // Read the IDs from TXT file
        // (Useless comment - better to move this piece of logic into a well-named method.)
        var txt = File.ReadAllText(file);
        var ids = txt.Split(',');
        foreach (var fileId in ids)
        {
            numbers.Add(int.Parse(fileId));
        }
    }
    else if (ex == "json")
    {
        // Read the IDs from JSON file
        // (Useless comment - better to move this piece of logic into a well-named method.)
        var txt = File.ReadAllText(file);
        numbers = JsonSerializer.Deserialize<List<int>>(txt);
    }
}

// A better approach is to have well-named methods instead of comments.
void SampleMethod(string name, string dir, string ex, int id)
{
    var file = dir + "/" + name + "." + ex;
    List<int> numbers = new List<int>();

    var fileContent = File.ReadAllText(file);
    if (ex == "txt")
    {
        numbers = ReadIdsFromText(fileContent);
    }
    else if (ex == "json")
    {
        numbers = ReadIdsFromJson(fileContent);
    }
}

List<int> ReadIdsFromText(string fileContext)
{
    List<int> numbers = new List<int>();

    var ids = fileContext.Split(',');
    foreach (var fileId in ids)
    {
        numbers.Add(int.Parse(fileId));
    }

    return numbers;
}

List<int> ReadIdsFromJson(string fileContext)
{
    return JsonSerializer.Deserialize<List<int>>(fileContext);
}

public class Person
{
    // Person's name
    // // (Useless comment, because the property name already states the same.)
    public string Name { get; }
    public int YearOfBirth { get; }

    // Constructor of the Person class
    // (Useless comment, because it is clear this is the constructor of the Person class.)
    public Person(string name, int yearOfBirth)
    {
        Name = name;
        YearOfBirth = yearOfBirth;
    }
}

// ### When comments add value 

// In this class, the comment helps understaning a complex regex.
public class EmailValidator
{
    public static bool IsValidEmail(string email)
    {
        // reasonable comment example: 
        // Simple email validation pattern
        // This pattern checks for valid characters,
        // the "@" symbol, domain names, and top-level domains (TLDs).

        // Examples of matching strings:
        // somebody@gmail.com
        // someone@yahoo.fr
        //
        // Examples of non-matching strings:
        // somebody@gmail
        // @yahoo.fr
        string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";

        return Regex.IsMatch(email, pattern);
    }
}

// In this class, one comment leads to the description of the implemented algorithm,
// and others help understaning the code.
public class AdaptiveHeapSort
{
    //reasonable comment example:
    //Implements the Adaptive Heap Sort algorithm
    // https://en.wikipedia.org/wiki/Adaptive_heap_sort
    public static void Sort(int[] input)
    {
        int length = input.Length;

        // Build the initial max-heap
        for (int i = length / 2 - 1; i >= 0; i--)
        {
            Heapify(input, length, i);
        }

        for (int i = length - 1; i > 0; i--)
        {
            // Swap the root element (maximum) with the last element
            int temp = input[0];
            input[0] = input[i];
            input[i] = temp;

            // Call heapify on the reduced heap
            Heapify(input, i, 0);
        }
    }

    private static void Heapify(int[] input, int n, int i)
    {
        int largest = i;
        int leftChild = 2 * i + 1;
        int rightChild = 2 * i + 2;

        // Find the largest element among the root, left child, and right child
        if (leftChild < n && input[leftChild] > input[largest])
        {
            largest = leftChild;
        }

        if (rightChild < n && input[rightChild] > input[largest])
        {
            largest = rightChild;
        }

        // If the largest element is not the root, swap them and recursively heapify the affected sub-tree
        if (largest != i)
        {
            int swap = input[i];
            input[i] = input[largest];
            input[largest] = swap;

            Heapify(input, n, largest);
        }
    }
}

// This class uses a summary comment, that will document the method in the IDE. 
public static class CollectionsProcessor
{
    /// <summary>
    /// Calculates the sum of the numbers in the specified collection.
    /// </summary>
    /// <param name="numbers">The numbers to sum.</param>
    /// <returns>The sum of the numbers.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="numbers"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// Thrown if the sum is greater than <see cref="int.MaxValue"/>
    /// or less than <see cref="int.MinValue"/>.
    /// </exception>
    public static int Sum(IEnumerable<int> numbers)
    {
        return numbers.Sum();
    }
}


// ##################
// helper types below

public record Point(double X, double Y);
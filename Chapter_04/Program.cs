Console.WriteLine("Press any key to close.");
Console.ReadKey();

// ### Why formatting matters 

// Both methods below work the same...
bool AreAllEven_BadFormatting(List<int> numbers)
{
    for (int i = 0; i < numbers.Count; i++) if (numbers[i] % 2 != 0) return false;
    return true;
}

// ...but this one is much easier to read.
bool AreAllEven(List<int> numbers)
{
    for (int i = 0; i < numbers.Count; i++)
    {
        if (numbers[i] % 2 != 0)
        {
            return false;
        }
    }

    return true;
}

// Similarly for those two methods. This one is not readable:
{
    bool IsWithinRange_BadFormatting(
        int min, int max, int value,
        bool isInclusive)
    {
    if (isInclusive)
    {
        return value >= min && value <= max;
    }else
    {
        return value > min && value < max;
    }
    }

    // And this one is much better:
    bool IsWithinRange(
        int min,
        int max,
        int value,
        bool isInclusive)
    {
        if (isInclusive)
        {
            return value >= min &&
                   value <= max;
        }
        else
        {
            return value > min &&
                   value < max;
        }
    }
}

// Indentation makes a big difference.
// This code is not readable:
bool AreAllEven2_BadFormatting(List<int> numbers)
{
for (int i = 0; i < numbers.Count; i++)
{
if (numbers[i] % 2 != 0)
{
    return false;
}
}
return true;
}

// This code is much more readable.
bool AreAllEven2(List<int> numbers)
{
    for (int i = 0; i < numbers.Count; i++)
    {
        if (numbers[i] % 2 != 0)
        {
            return false;
        }
    }

    return true;
}

// Braces can alter the behavior of the code.
// Without braces, only the first statement belongs to the loop: 
var numbers = new List<int> { 1, 2, 3 };
for (int i = 0; i < numbers.Count; i++)
    Console.WriteLine("Do something");
Console.WriteLine("Do something else");

// If both statements are intended to repeat, the block should be explicit: 
for (int i = 0; i < numbers.Count; i++)
{
    Console.WriteLine("Do something");
    Console.WriteLine("Do something else");
}

// ### Breaking long lines clearly 

// This code is hard to follow:
var gameArray = new char[3, 3];
int j = 0;
char c = 'X';
if (gameArray[j, 0] == c && gameArray[j, 1] == c && gameArray[j, 2] == c)
{
    // do something
}

// This code is much cleaner:
if (gameArray[j, 0] == c &&
    gameArray[j, 1] == c &&
    gameArray[j, 2] == c)
{
    // do something
}

//Breaking a long parameter list can make the method signature much more readable:
{
    bool IsWithinRange_BadFormatting(int min, int max, int value, bool isInclusive)
    {
        throw new NotImplementedException();
    }

    bool IsWithinRange(
        int min,
        int max,
        int value,
        bool isInclusive)
    {
        throw new NotImplementedException();
    }
}

// The same goes for breaking a long method chain:

//This is less readable:
var result_badFormatting = numbers.Where(number => number % 2 != 0).Select(number => number * number).Take(3);

//This is more readable:
var result = numbers
    .Where(number => number % 2 != 0)
    .Select(number => number * number)
    .Take(3);


// ### Using vertical space to organize code 

// In this method, blank lines separate three sections:
// 1) Variables declarations
// 2) Calculation logic
// 3) Result presentation
void CountEvenAndOdd(List<int> numbers)
{
    int totalEven = 0;
    int totalOdd = 0;

    for (int i = 0; i < numbers.Count; i++)
    {
        Console.WriteLine($"Processing number at index {i}");

        if (numbers[i] % 2 == 0)
        {
            totalEven++;
        }
        else
        {
            totalOdd++;
        }
    }

    Console.WriteLine($"Total even numbers: {totalEven}");
    Console.WriteLine($"Total odd numbers: {totalOdd}");
}

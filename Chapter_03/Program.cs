using System.Data;
using System.Text.Json;

// ### Method fundamentals

// In this method call, 5 and 3 are method's arguments.
var sum = Add(5, 3);

// This is a simple method taking two int parameters (a and b) and returning an int.
int Add(int a, int b) => a + b;

// Methods can be assigned to variables.
// Here, someFunction is a variable currently assigned to the IsDivisible method.
// Func<int, int, bool> is a generic delegate type that can represent any method
// that takes two int parameters and returns a bool.
Func<int, int, bool> someFunction = IsDivisibleBy;

bool IsDivisibleBy(int number, int divisor) => number % divisor == 0;

var numbers = new int[] { 1, 7, 19, 4, 20, 0 };
// Where takes as a parameter any method that takes a number and returns a bool.
// Here, we pass a lambda expression that checks whether the given number is larger than 10.
var largeNumbers = numbers.Where(number => number > 10);


// ### Designing clear method signatures 
var fileWriter = new FileWriter();

// This call is ambiguous, because it's not clear what "someString" represents.
fileWriter.Save("someString");

//This call makes it clear that "someString" will be appended to a file.
fileWriter.AppendText("someString");

//This call makes it clear that "someString" will replace file's content.
fileWriter.ReplaceContentWith("someString");

//This call makes it clear that "someString" is the name of the file we will write to.
fileWriter.WriteTo("someString");


// ### Choosing the right number of parameters 

// The more parameter the method has, the easier it is to make a mistake when calling it.
var firstNumber = numbers.First(); // Impossible to call with an invalid argument.
var itemAtIndexZero = numbers.ElementAt(0); // A valid argument is provided here... 
var invalidItem = numbers.ElementAt(-1); // ...but not here.

// It's hard to reason about a method call when it takes so many arguments:
var databaseConnector = new DatabaseConnector();
databaseConnector.ConnectToDatabase(
    DbType.PostgreSql,
    "Main",
    "Sol",
    new Cluster { Name = "BeeHive" },
    "user123",
    "Top$ecret",
    1000);

// ### Using Boolean parameters carefully 
var point1 = new Point(3, 4);
var point2 = new Point(5, 5);

// It's hard to say what's the difference between those calls,
// and what the meaning of the last argument is.
var distance1 = CalculateDistance(point1, point2, true);
var distance2 = CalculateDistance(point1, point2, false);

// It turns out this method calculates distance in either kilometers or miles,
// depending on the last argument. 
// It's best to have two separate methods: one for distance calculation, 
// and the other for converting between units.
var distanceInKm = CalculateDistanceInKilometers(point1, point2);
var distanceInMiles = UnitConverter.KilometersToMiles(distanceInKm);

// Not every Boolean parameter is a problem.
// A value such as isAdmin can be legitimate when it is simply data
// used by a single decision rather than a flag that switches the method
// between unrelated jobs
var isAuthorized = new Authorizer().IsAuthorized("userName", isAdmin: true);


// ### Avoiding parameter-count anti-patterns 

// Not every method with three or more parameters is bad.
// Some simply need that many inputs, like this  one:
var areEqual = AreEqualWithinTolerance(1.002f, 1.0001f, 0.000001f);

// It's easier to use a method that clearly states all inputs it needs...
var totalPrice = OrderPriceCalculator.CalculateTotalPrice(
    quantity: 5,
    unitPrice: 9.99m,
    tax: 0.23m);

// ...than a method that hides some of its required inputs as class state:
var calculator = new OrderPriceCalculator(0.23m);
var totalPriceHiddenTax = calculator.CalculateTotalPrice(
    quantity: 5,
    unitPrice: 9.99m);

Console.WriteLine("Press any key to close.");
Console.ReadKey();


// ### Keeping methods small and focused 

// This method does two things instead of one:
// It both calculates the distance and performs unit conversion.
double CalculateDistance(Point point1, Point point2, bool shouldBeAsMiles)
{
    double deltaX = point2.X - point1.X;
    double deltaY = point2.Y - point1.Y;
    var distanceInKm = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

    if (shouldBeAsMiles)
    {
        const double kilometersToMilesConversionFactor = 0.621371;
        return distanceInKm * kilometersToMilesConversionFactor;
    }

    return distanceInKm;
}

// This method has one, clearly defined job: calculating distance.
double CalculateDistanceInKilometers(Point point1, Point point2)
{
    double deltaX = point2.X - point1.X;
    double deltaY = point2.Y - point1.Y;
    return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
}

// This method simply needs 3 parameters.
// Trying to reduce this number would make the code worse.
bool AreEqualWithinTolerance(float a, float b, float tolerance)
{
    return Math.Abs(a - b) <= tolerance;
}

// This method does one job: coordinating the retrieval of user data
// The particular steps are delegated to other methods.
List<User> ReadUsersData()
{
    var apiConnection = ConnectToApi("https://someApi/", "api/users");
    var usersAsJson = apiConnection.ReadAll();
    return JsonSerializer.Deserialize<List<User>>(usersAsJson);
}

IApiConnection ConnectToApi(string rootUrl, string resource)
{
    throw new NotImplementedException();
}

// This method, although short, does two things:
// building a full file path and writing to a text file.
{
    void SaveToFile(string fileName, string content)
    {
        File.WriteAllText(fileName + ".txt", content);
    }
}
{
    // This method  filters empty strings,
    // joins the remaining strings, and writes the result to a file.
    // It would be better to refactor it into more focused methods.
    void SaveToFile(List<string> words, string filePath)
    {
        var nonEmptyWords = new List<string>();
        foreach (var word in words)
        {
            if (!string.IsNullOrEmpty(word))
            {
                nonEmptyWords.Add(word);
            }
        }

        string text = string.Join(Environment.NewLine, nonEmptyWords);
        File.WriteAllText(filePath, text);
    }

    // Now each method has a clearly defined job.
    void SaveNonEmptyToFile(List<string> words, string filePath)
    {
        var nonEmptyWords = GetNonEmptyOnly(words);
        var textToBeSaved = string.Join(Environment.NewLine, nonEmptyWords);
        File.WriteAllText(filePath, textToBeSaved);
    }

    List<string> GetNonEmptyOnly(List<string> words)
    {
        var nonEmptyWords = new List<string>();
        foreach (var word in words)
        {
            if (!string.IsNullOrEmpty(word))
            {
                nonEmptyWords.Add(word);
            }
        }
        return nonEmptyWords;
    }
}


// ### Using pure functions where they fit 

// Max is pure because its result is completely determined by a and b.  
int Max(int a, int b)
{
    return a >= b ? a : b;
}

//GetCurrentTime is impure, because it result is different each time it's called.
string GetCurrentTime(string format)
{
    return DateTime.Now.ToString(format);
}

class ItemsStorage
{
    private List<Item> _items = new();

    // FindItemWithName is not pure because it depends on the _items field.
    // The field can change independently of the arguments,
    // so the same argument may produce a different result on the next call.
    public Item FindItemWithName(string name)
    {
        return _items.FirstOrDefault(item => item.Name == name);
    }
}

public class DatabaseConnector
{
    public void ConnectToDatabase(
        DbType type,
        string databaseName,
        string serverName,
        Cluster serverCluster,
        string userName,
        string password,
        int timeout)
    {
        throw new NotImplementedException();
    }

    // When a method takes a lot of arguments, consider grouping them
    // into dedicated data structures.
    public IDatabaseConnection ConnectToDatabase(
        string userName,
        string password,
        string databaseName,
        string databaseServerName,
        string databaseClusterName)
    {
        throw new NotImplementedException();
    }

    // This method has only two arguments, each easy to understand and construct.
    IDatabaseConnection ConnectToDatabase(
        UserCredentials userCredentials,
        DatabaseIdentity databaseIdentity)
    {
        throw new NotImplementedException();
    }
}
public struct DatabaseIdentity
{
    public string DatabaseName;
    public string DatabaseServerName;
    public string DatabaseClusterName;
}

public struct UserCredentials
{
    public string UserName;
    public string Password;
}

public class GameState
{
    private readonly IGameDataWriter _gameDataWriter;

    public GameState(IGameDataWriter gameDataWriter)
    {
        _gameDataWriter = gameDataWriter;
    }

    // This method takes too many parameters, because it does too many things.
    // (It both builds the file path and saves the GameData object.
    public void SaveGame(
        GameData data,
        string saveFileName,
        string saveFileExtension)
    {
        var finalPath = saveFileName + "." + saveFileExtension;
        _gameDataWriter.WriteTo(finalPath, data);
    }

    // It's better to split this method into two, each focused on one task.
    // This will reduce the number of parameters for each method and make them
    // smaller, more focused, and easily reusable.
    public void SaveGame(GameData data, string saveFileName)
    {
        _gameDataWriter.WriteTo(saveFileName, data);
    }

    public string BuildFileName(
        string fileNameWithoutExtension, 
        string extension)
    {
        return fileNameWithoutExtension + "." + extension;
    }
}

public struct Circle_BadDesign
{
    public float X { get; }
    public float Y { get; }
    public float Radius { get; }

    // This constructor needs 3 parameters, which is a lot.
    // It's better to group X and Y into a Point struct.
    public Circle_BadDesign(float x, float y, float radius)
    {
        X = x;
        Y = y;
        Radius = radius;
    }
}

public struct Circle
{
    public Point Center { get; }
    public float Radius { get; }

    // This struct takes only 2 parameters in its constructor,
    // and the new Point type can now be reused.
    public Circle(Point center, float radius)
    {
        Center = center;
        Radius = radius;
    }
}

public record Point(float X, float Y);

public static class UnitConverter
{
    public static double KilometersToMiles(double distanceInKm)
    {
        const double kilometersToMilesConversionFactor = 0.621371;
        return distanceInKm * kilometersToMilesConversionFactor;
    }
}

public class Authorizer
{
    private readonly List<string> _authorizedUsers = [];

    public bool IsAuthorized(string userName, bool isAdmin)
    {
        if (isAdmin)
        {
            return true;
        }

        return _authorizedUsers.Contains(userName);
    }
}

public class OrderPriceCalculator
{
    private readonly decimal _tax;

    public OrderPriceCalculator(decimal tax)
    {
        _tax = tax;
    }

    // Here, the number of parameters is justified.
    // This method simply needs all those inputs.
    public static decimal CalculateTotalPrice(
        int quantity, decimal unitPrice, decimal tax)
    {
        return quantity * unitPrice * (1 + tax);
    }

    // Making one input part of the class state makes the code worse, not better.
    public decimal CalculateTotalPrice(int quantity, decimal unitPrice)
    {
        return quantity * unitPrice * (1 + _tax);
    }
}

// ##################
// helper types below

public class FileWriter
{
    public void Save(string input)
    {
        throw new NotImplementedException();
    }

    public void AppendText(string text)
    {
        throw new NotImplementedException();
    }

    public void ReplaceContentWith(string content)
    {
        throw new NotImplementedException();
    }

    public void WriteTo(string fileName)
    {
        throw new NotImplementedException();
    }
}

public record Cluster
{
    public string Name { get; init; }
}

public enum DbType
{
    MsSql,
    PostgreSql
}

public record GameData
{

}

public interface IGameDataWriter
{
    void WriteTo(string finalPath, GameData data);
}

public interface IDatabaseConnection
{

}

public record User
{

}

public interface IApiConnection
{
    string ReadAll();
}

public record Item
{
    public string Name { get; init; }
}
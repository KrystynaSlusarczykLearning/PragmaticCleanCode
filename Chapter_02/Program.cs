using System.Text;

var user = new User();
var file = new File();
var account = new Account();

// ### Correct naming of Boolean properties
if (user.IsEnabled) { /*...*/ }
if (file.HasBeenSavedSuccessfully) { /*...*/ }
if (account.CanBeDeleted) { /*...*/ }

// Boolean properties naming:
// Prefer this: 
if (!user.IsEnabled) { /*...*/ }

// Over this: 
if (user.IsNotEnabled) { /*...*/ }


// ### Meaningful names

// This method removes whitespace from the end of input, but its name does not express that.
string Clear(string input) { throw new NotImplementedException(); }

// A better name carries the explanation itself:
string RemoveTrailingWhitespaces(string input) { throw new NotImplementedException(); }

// Or, using familiar string terminology: 
string TrimTrailingWhitespaces(string input) { throw new NotImplementedException(); }


// ### Choosing an appropriate name length 

// All variables represent a collection with adults only.
// In this case, the shortest name is the best, because it carries
// the same information as the longer alternatives, just more concisely.
var onlyPeopleOlderThan18Years = new List<Person>();
var peopleOlderThan18Years = new List<Person>();
var adultPeople = new List<Person>();
var adults = new List<Person>();


// ### Matching singular and plural forms
{
    // Misleading: the name implies one ID. 
    var inactiveUserId = new[] { 4, 12, 16 };
}
{
    // Clear: the value is a collection. 
    var inactiveUserIds = new[] { 4, 12, 16 };

    // Also clear if CompositeId is a single domain value. 
    var inactiveUserId = new CompositeId(4, 12, 16);
}

Console.WriteLine("Press any key to close.");
Console.ReadKey();

// This name is correct because this method handles one Vector.
Vector NormalizeVector(Vector vector) { throw new NotImplementedException(); }

// Once changed to handle multiple Vectors,
// the name should be changed too to match the cardinality.
List<Vector> NormalizeVectors(List<Vector> vectors) { throw new NotImplementedException(); }


// ### Using natural language in code 
var vector = new Vector(10, 12);
var otherVector = new Vector(10, 10);

// This is harder to read:
if (vector.IsShorter(otherVector)) { throw new NotImplementedException(); }

// This is easy to read:
if (vector.IsShorterThan(otherVector)) { throw new NotImplementedException(); }

var point = new Point(5, 6);

//This is ambiguous, because it's not clear what kind of transformation it is.
var newPointTransformed = point.Transform(1, 2);

// This is clear, precise and easy to read.
var newPointMoved = point.MoveBy(1, 2);


// ### Avoiding Hungarian notation and type-based names 

// Avoid type prefixes 
int intAge;
string strLastName;
List<int> numbersList;

// Prefer domain names. 
int age;
string lastName;
List<int> numbers;


// ### Using abbreviations carefully 

string addr;       // Requires interpretation. 
string address;    // Immediate meaning. 

var prod = new Prod();       // Product or production? 
var product = new Product(); // Clear in this context. 

// Lambda expressions also benefit from using full names instead of abbreviations.
Func<Address, string> func1 = (a) => $"{a.Number} {a.Street} street.";
Func<Address, string> func2 = (address) => $"{address.Number} {address.Street} street.";

Console.WriteLine("Press any key to close.");
Console.ReadKey();

public class Filterer
{
    // poor names
    public List<string> Filter(int l, List<string> items)
    {
        var res = new List<string>();
        foreach (var i in items)
        {
            if (i.Length < l)
            {
                res.Add(i);
            }
        }
        return res;
    }

    // good names
    public List<string> GetWordsShorterThan(int length, List<string> words)
    {
        var result = new List<string>();
        foreach (var word in words)
        {
            if (word.Length < length)
            {
                result.Add(word);
            }
        }
        return result;
    }
}

// This class combines unrelated responsibilites, which makes it hard to name well.
class EmailManager
{
    public void Send() { throw new NotImplementedException(); }

    public void Create() { throw new NotImplementedException(); }
}

// After splitting EmailManager, new classes can be named more accurately.
class EmailSender
{
    public void Send() { throw new NotImplementedException(); }
}

class AccountCreator
{
    public void Create() { throw new NotImplementedException(); }
}

class Person
{
    public Address HomeAddress { get; init; }

    // This name is too long.
    // Better to have the IsValid method in the Address type.
    bool IsHomeAddressValid() { throw new NotImplementedException(); }

    public void Example(Person person)
    {
        if (person.IsHomeAddressValid()) // This name is too long.
        {

        }

        if (person.HomeAddress.IsValid()) // Those names are better.
        {

        }
    }
}

class Address
{
    public string Number { get; init; }
    public string Street { get; init; }

    public bool IsValid()
    {
        throw new NotImplementedException();
    }
}

class TicketsProcessor
{
    private readonly IDocumentsReader _documentsReader;
    private readonly IFileWriter _fileWriter;
    private readonly Folder _ticketsFolder;

    public TicketsProcessor(
        IDocumentsReader documentsReader,
        IFileWriter fileWriter,
        Folder ticketsFolder)
    {
        _documentsReader = documentsReader;
        _fileWriter = fileWriter;
        _ticketsFolder = ticketsFolder;
    }

    // Reading this method top to bottom tells the whole story:
    // ticket documents are read, then processed, then the aggregated result is written.
    public void Run()
    {
        var stringBuilder = new StringBuilder();
        var ticketDocuments = _documentsReader.Read(_ticketsFolder);

        foreach (var document in ticketDocuments)
        {
            var lines = ProcessDocument(document);
            stringBuilder.AppendLine(
                string.Join(Environment.NewLine, lines));
        }

        _fileWriter.Write(
            stringBuilder.ToString(),
            _ticketsFolder,
            "aggregatedTickets.txt");
    }

    private IEnumerable<string> ProcessDocument(Document document)
    {
        throw new NotImplementedException();
    }
}

public class UsersStorage
{
    private readonly IUsersRepository _usersRepository;

    public UsersStorage(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    // This method's name overstates what the method guarantees.
    // It saves only when the validation succeeds.
    public void SaveUser(User user)
    {
        if (user.Email is not null)
        {
            _usersRepository.Save(user);
        }
    }

    // Better to have two separate methods for saving and validation, 
    // and use them in an "if" statement like this:
    //
    // if (IsValid(user)) 
    // { 
    //    Save(user);
    // }

    public void Save(User user)
    {
        _usersRepository.Save(user);
    }
    
    public bool IsValid(User user)
    {
        return user.Email is not null;
    } 
}

public class PersonalDataRepository_BadNames
{
    // The same operation goes by three names: Get, Fetch, and Retrieve.
    public DateTime GetDateOfBirth(int personId) { throw new NotImplementedException(); }
    public string FetchCityOfBirth(int personId) { throw new NotImplementedException(); }
    public string RetrieveSocialSecurityNumber(int personId) { throw new NotImplementedException(); }
}

public class PersonalDataRepository
{
    // The fix is to pick one word and use it everywhere.
    // Get is the shortest and the most common, so it is the natural choice here.
    public DateTime GetDateOfBirth(int personId) { throw new NotImplementedException(); }
    public string GetCityOfBirth(int personId) { throw new NotImplementedException(); }
    public string GetSocialSecurityNumber(int personId) { throw new NotImplementedException(); }
}

// ##################
// helper types below

public record User()
{
    public bool IsEnabled { get; init; }
    public bool IsNotEnabled { get; init; } // Avoid negation in Boolean-based names.
    public string Email { get; init; }
}

public record File()
{
    public bool HasBeenSavedSuccessfully { get; init; }
}

public record Account()
{
    public bool CanBeDeleted { get; init; }
}

public interface IDocumentsReader
{
    IEnumerable<Document> Read(Folder ticketsFolder);
}

public interface IFileWriter
{
    void Write(string content, Folder folder, string fileName);
}

public interface IUsersRepository
{
    void Save(User user);
}

public record Folder
{

}

public record Document
{

}

public record CompositeId(params int[] Components);

public record Vector(int X, int Y)
{
    internal bool IsShorter(Vector otherVector)
    {
        throw new NotImplementedException();
    }

    internal bool IsShorterThan(Vector otherVector)
    {
        throw new NotImplementedException();
    }
}

public record Point(int X, int Y)
{
    internal object MoveBy(int x, int y)
    {
        throw new NotImplementedException();
    }

    internal object Transform(int x, int y)
    {
        throw new NotImplementedException();
    }
}

public record Prod
{

}

public record Product
{

}
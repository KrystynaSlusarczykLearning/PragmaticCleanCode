Console.WriteLine("Press any key to close.");
Console.ReadKey();

// ### Understanding and reducing coupling between classes 

// The Subscribers and NewsletterSender classes are tightly coupled,
// because NewsletterSender knows the implementation details of Subscribers,
// and can even modify its internal state (for example, set elements within Items to null).
// If Subscribers would like to change its array to a HasSet,
// NewsletterSender would stop working.
public class Subscribers_TightCoupling
{
    public Subscriber[] Items { get; }

    public Subscribers_TightCoupling(Subscriber[] items)
    {
        Items = items;
    }
}

public class NewsletterSender_TightCoupling
{
    private readonly Subscribers_TightCoupling _subscribers;

    public NewsletterSender_TightCoupling(Subscribers_TightCoupling subscribers)
    {
        _subscribers = subscribers;
    }

    public void SendTo(bool premiumSubscribersOnly)
    {
        for (int i = 0; i < _subscribers.Items.Length; i++)
        {
            if (!premiumSubscribersOnly ||
                _subscribers.Items[i].IsPremium)
            {
                Console.WriteLine(
                    $"Newsletter sent to {_subscribers.Items[i].Email}");
            }
        }
    }
}

// The solution is to make Subscribers hide its implementation details,
// in this case, behind the IEnumerable interface.
// Now Subscribers can switch its HasSet to any other collection type,
// and no other type will be affected by this change.
public class Subscribers
{
    private readonly HashSet<Subscriber> _items;
    public IEnumerable<Subscriber> Items => _items;

    public Subscribers(HashSet<Subscriber> items)
    {
        _items = items;
    }
}

public class NewsletterSender
{
    private readonly Subscribers _subscribers;

    public NewsletterSender(Subscribers subscribers)
    {
        _subscribers = subscribers;
    }

    public void SendTo(bool premiumSubscribersOnly)
    {
        foreach (var subscriber in _subscribers.Items)
        {
            if (!premiumSubscribersOnly || subscriber.IsPremium)
            {
                Console.WriteLine(
                    $"Newsletter sent to {subscriber.Email}");
            }
        }
    }
}

// ### Applying the Law of Demeter 

public class FileAccess
{
    private string _mainDirectory;

    public FileAccess(ApplicationContext context)
    {
        // This breaks the Law of Demeter - FileAccess must know all the details about
        // how to access main directory from ApplicationContext.
        _mainDirectory = context.GetAppData().GetDirectory().Main;
    }


    // The solution is simply to ask for the thing FileAccess needs in its constructor.
    public FileAccess(string mainDirectory)
    {
        _mainDirectory = mainDirectory;
    }

    //other methods for file access
}

// ### Designing for high cohesion 

// This class is highly cohesive: Every member either works with the same _pets collection
// or builds on another operation in the class.
// There is no obvious boundary along which the type should be divided. 
class PetsCollection
{
    private readonly List<Pet> _pets = new();

    public int Count => _pets.Count;

    public void Add(Pet pet)
    {
        _pets.Add(pet);
    }

    public IEnumerable<PetType> GetCurrentlyStoredTypes()
    {
        return _pets
            .Select(pet => pet.PetType)
            .Distinct();
    }

    public bool Contains(PetType petType)
    {
        return GetCurrentlyStoredTypes()
            .Any(type => type == petType);
    }
}

// This class is not cohesive, and it could be easily divided into two:
// One focused on house pricing, and another on owner notifications
public class HousePricerLowCohesion
{
    private IOwnersDatabase _ownersDatabase;
    private decimal _dollarsPerSquareMeter;

    public HousePricerLowCohesion(
        decimal dollarsPerSquareMeter,
        IOwnersDatabase ownersDatabase)
    {
        _dollarsPerSquareMeter = dollarsPerSquareMeter;
        _ownersDatabase = ownersDatabase;
    }

    public decimal GetPrice(House house)
    {
        return
            _dollarsPerSquareMeter *
            (decimal)house.Area *
            GetPriceMultiplierBasedOnFloors(house.Floors);
    }

    private decimal GetPriceMultiplierBasedOnFloors(int floors)
    {
        return floors switch { 1 => 1m, 2 => 1.5m, _ => 1.6m };
    }

    public void SendPriceToOwner(House house)
    {
        Console.WriteLine($"Sending price {GetPrice(house)}" +
            $" to {FindOwnerEmail(house.Address)}");
    }

    private string FindOwnerEmail(string address)
    {
        return _ownersDatabase.GetEmailByAddress(address);
    }
}

// The two classes below are highly cohesive. Splitting any of them would feel forced.
public class HousePricer
{
    private decimal _dollarsPerSquareMeter;

    public HousePricer(decimal dollarsPerSquareMeter)
    {
        _dollarsPerSquareMeter = dollarsPerSquareMeter;
    }

    public decimal GetPrice(House house)
    {
        return
            _dollarsPerSquareMeter *
            (decimal)house.Area *
            GetPriceMultiplierBasedOnFloors(house.Floors);
    }

    private decimal GetPriceMultiplierBasedOnFloors(int floors)
    {
        return floors switch { 1 => 1m, 2 => 1.5m, _ => 1.6m };
    }
}

public class OwnerNotifier
{
    private IOwnersDatabase _ownersDatabase;
    public OwnerNotifier(IOwnersDatabase ownersDatabase)
    {
        _ownersDatabase = ownersDatabase;
    }

    public void SendPriceToOwner(decimal price, string address)
    {
        Console.WriteLine($"Sending price {price}" +
            $" to {FindOwnerEmail(address)}");
    }

    private string FindOwnerEmail(string address)
    {
        return _ownersDatabase.GetEmailByAddress(address);
    }
}

// ### Applying the DRY principle to knowledge 

public class OnlineStore_BreakingDRY
{
    // The below two methods break DRY
    // because we define twice that the customer has 30 days for return.
    public DateTime GetReturnDateDeadline(DateTime purchaseDate)
    {
        return purchaseDate.AddDays(30);
    }

    public bool IsAfterReturnDateDeadline(DateTime purchaseDate)
    {
        return (DateTime.Now - purchaseDate).TotalDays > 30;
    }

    // This is a slight code duplication, but it does not break DRY.
    public void CommitOrder(Order order)
    {
        if (string.IsNullOrEmpty(order.CustomerId))
        {
            throw new ArgumentException($"The CustomerId must not be empty");
        }
        if (string.IsNullOrEmpty(order.ProductId))
        {
            throw new ArgumentException($"The ProductId must not be empty");
        }

        // Saving to database here...
        Console.WriteLine("Order committed and saved to database");
    }
}

public class OnlineStore_NotBreakingDRY
{
    private const int DaysForReturn = 30;
    private const int DaysForRefund = 30;

    // We have slight code duplication here, but at least we don't break DRY.
    public DateTime GetReturnDateDeadline(DateTime purchaseDate)
    {
        return purchaseDate.AddDays(DaysForReturn);
    }

    public bool IsAfterReturnDateDeadline(DateTime purchaseDate)
    {
        return IsBeforeNow(GetReturnDateDeadline(purchaseDate));
    }

    public DateTime GetRefundDateDeadline(DateTime returnDate)
    {
        return returnDate.AddDays(DaysForRefund);
    }

    public bool IsAfterRefundDateDeadline(DateTime returnDate)
    {
        return IsBeforeNow(GetRefundDateDeadline(returnDate));
    }

    private bool IsBeforeNow(DateTime dateTime)
    {
        return dateTime < DateTime.Now;
    }

    public void CommitOrder(Order order)
    {
        Validate(order.CustomerId, nameof(order.CustomerId));
        Validate(order.ProductId, nameof(order.ProductId));

        //saving to database here...
        Console.WriteLine("Order committed and saved to database.");
    }

    private void Validate(string idToBeValidated, string propertyName)
    {
        if (string.IsNullOrEmpty(idToBeValidated))
        {
            throw new ArgumentException($"The {propertyName} must not be empty.");
        }
    }
}

// ### Composition vs inheritance

// Composition approach - makes the code loosely coupled.
class PersonalDataFormatter
{
    private readonly IPersonalDataReader _personalDataReader;

    public PersonalDataFormatter(IPersonalDataReader personalDataReader)
    {
        _personalDataReader = personalDataReader;
    }

    public string Format()
    {
        var people = _personalDataReader.ReadPeople();
        return string.Join("\n",
            people.Select(p => $"{p.Name} born in" +
            $" {p.Country} on {p.YearOfBirth}"));
    }
}

class DatabaseSourcedPersonalDataReader : IPersonalDataReader
{
    public IEnumerable<Person> ReadPeople()
    {
        Console.WriteLine("Reading from database");
        return new List<Person>
        {
            new Person("John", 1982, "USA"),
            new Person("Aja", 1992, "India"),
            new Person("Tom", 1954, "Australia"),
        };
    }
}

class ExcelSourcedPersonalDataReader : IPersonalDataReader
{
    public IEnumerable<Person> ReadPeople()
    {
        Console.WriteLine("Reading from an Excel file");
        return new List<Person>
        {
            new Person("Martin", 1972, "France"),
            new Person("Aiko", 1995, "Japan"),
            new Person("Selene", 1944, "Great Britain"),
        };
    }
}

// Inheritance approach - creates tighter coupling between types.
abstract class PersonalDataFormatter_Inheritance
{
    public string Format()
    {
        var people = ReadPeople();
        return string.Join("\n",
            people.Select(p => $"{p.Name} born in" +
            $" {p.Country} on {p.YearOfBirth}"));
    }

    public abstract IEnumerable<Person> ReadPeople();
}

class DatabaseSourcedPersonalDataFormatter_Inheritance : PersonalDataFormatter_Inheritance
{
    public override IEnumerable<Person> ReadPeople()
    {
        Console.WriteLine("Reading from database");
        return new List<Person>
        {
            new Person("John", 1982, "USA"),
            new Person("Aja", 1992, "India"),
            new Person("Tom", 1954, "Australia"),
        };
    }
}

class ExcelSourcedPersonalDataFormatter_Inheritance : PersonalDataFormatter_Inheritance
{
    public override IEnumerable<Person> ReadPeople()
    {
        Console.WriteLine("Reading from an Excel file");
        return new List<Person>
        {
            new Person("Martin", 1972, "France"),
            new Person("Aiko", 1995, "Japan"),
            new Person("Selene", 1944, "Great Britain"),
        };
    }
}


// ##################
// helper types below

public class Subscriber
{
    public string Email { get; init; }
    public bool IsPremium { get; init; }
}

public class ApplicationContext
{
    public AppData GetAppData()
    {
        return new AppData();
    }
}

public class AppData
{
    public Directory GetDirectory()
    {
        return new Directory { Main = "C:/someFolder" };
    }
}

public class Directory
{
    public string Main { get; init; }
}


public record Person(string Name, int YearOfBirth, string Country);
public record Pet(string Name, PetType PetType, float Weight);
public enum PetType { Cat, Dog, Fish }
public record House(string Address, double Area, int Floors);

public interface IOwnersDatabase
{
    string GetEmailByAddress(string address);
}

public class Order
{
    public string CustomerId { get; }
    public string ProductId { get; }

    public Order(string customerId, string productId)
    {
        CustomerId = customerId;
        ProductId = productId;
    }
}

interface IPersonalDataReader
{
    IEnumerable<Person> ReadPeople();
}

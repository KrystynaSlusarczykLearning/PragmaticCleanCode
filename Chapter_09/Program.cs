Console.WriteLine("Press any key to close.");
Console.ReadKey();

// This class breaks the DIP, because is depends directly on FastWheels,
// and not on an abstracion.
class OnlineBookstore_BreakingDIP
{
    private readonly FastWheels _fastWheelsService;

    public OnlineBookstore_BreakingDIP(FastWheels fastWheelsService)
    {
        _fastWheelsService = fastWheelsService;
    }

    public void Send(int bookId, int customerId)
    {
        var package = new Package(bookId); // create package with book 
        var address = new Address(customerId); //get customer's address
        _fastWheelsService.DeliverSmallItem(package, address);
    }
}

// The solution is to make the OnlineBookstore depend on an abstraction,
// not on a concrete type.
class OnlineBookstore
{
    private readonly IDelivery _deliveryService;

    // Dependency Injection in action - delivery service is injected via the constructor,
    // not created within this class.
    public OnlineBookstore(IDelivery deliveryService)
    {
        _deliveryService = deliveryService;
    }

    public void Send(int bookId, int customerId)
    {
        var package = new Package(bookId); // create package with book 
        var address = new Address(customerId); //get customer's address
        _deliveryService.Deliver(package, address);
    }
}


public interface IDelivery
{
    void Deliver(Package package, Address address);
}

public class FastWheels
{
    public void DeliverSmallItem(Package package, Address address)
    {
        // deliver a small item
    }
}



// Dynamic dependency creation - the implementor of IWeatherDataAccess is created,
// once the country parameter it needs is available.

public class WeatherDataPrinter
{
    private IWeatherDataAccessFactory _weatherDataAccessFactory;

    public WeatherDataPrinter(IWeatherDataAccessFactory weatherDataAccessFactory)
    {
        _weatherDataAccessFactory = weatherDataAccessFactory;
    }

    public void Print()
    {
        Console.WriteLine("Please select a country:");
        var country = Console.ReadLine();

        var weatherDataAccess = _weatherDataAccessFactory.Create(country);
        var weatherData = weatherDataAccess.GetCurrentWeather();
        Console.WriteLine($"The weather in {country} is: {weatherData}");
    }
}

public interface IWeatherDataAccessFactory
{
    IWeatherDataAccess Create(string country);
}

public class WeatherDataAccessFactory : IWeatherDataAccessFactory
{
    public IWeatherDataAccess Create(string country)
    {
        return new WeatherDataAccess(country);
    }
}

public interface IWeatherDataAccess
{
    string GetCurrentWeather();
}

public class WeatherDataAccess : IWeatherDataAccess
{
    private string _country;

    public WeatherDataAccess(string country)
    {
        _country = country;
    }

    public string GetCurrentWeather()
    {
        //access weather data for given country
        return string.Empty;
    }
}

// Refactoring towards compliance with DIP.
// This class breaks the DIP, because it depends on a concrete TextFilePopulationDataProvider.
// Moreover, it creates it itself, instead of using Dependency Injection.
public class PopulationStatsPrinter_BreakingDIP
{
    public void PrintAverageAge()
    {
        List<Person> people =
            new TextFilePopulationDataProvider("people.txt").GetAll();

        var averageAge = people.Average(person => person.Age);
        Console.WriteLine($"The average age is {averageAge}.");
    }
}

// A better approach is to depend an an abstraction, and use Dependency Injection:
public class PopulationStatsPrinter
{
    private IPopulationDataProvider _populationDataProvider;

    public PopulationStatsPrinter(IPopulationDataProvider populationDataProvider)
    {
        _populationDataProvider = populationDataProvider;
    }

    public void PrintAverageAge()
    {
        List<Person> people = _populationDataProvider.GetAll();
        var averageAge = people.Average(person => person.Age);
        Console.WriteLine($"The average age is {averageAge}.");
    }
}

public interface IPopulationDataProvider
{
    List<Person> GetAll();
}

public class TextFilePopulationDataProvider : IPopulationDataProvider
{
    private readonly string _filePath;

    public TextFilePopulationDataProvider(string filePath)
    {
        _filePath = filePath;
    }

    public List<Person> GetAll()
    {
        var people = new List<Person>();

        var lines = File.ReadAllLines(_filePath);

        foreach (var line in lines)
        {
            people.Add(ProcessLine(line));
        }

        return people;
    }

    private static Person ProcessLine(string line)
    {
        var data = line.Split(',');

        return new Person
        {
            Id = int.Parse(data[0]),
            FirstName = data[1],
            LastName = data[2],
            Age = int.Parse(data[3])
        };

    }
}

// ##################
// helper types below

public record Package
{
    public Package(int itemId)
    {
    }
}

public record Address
{
    public Address(int customerId)
    {
    }
}

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}
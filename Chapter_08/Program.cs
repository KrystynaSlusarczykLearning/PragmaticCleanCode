using System.Text.Json;

Console.WriteLine("Press any key to close.");
Console.ReadKey();

// This design breaks the ISP, because adding the Write method to the interface
// forces all implementers, including CensusPersonalDataAccess, to implement it.
// But CensusPersonalDataAccess cannot do it reasonably, and it is forced to provide
// a stub implementation.
public interface IPersonalDataAccess
{
    string Read(int id);
    void Write(int id, string firstName, string lastName);
}

public class TextFilePersonalDataAccess_BreakingIsp : IPersonalDataAccess
{
    public  string FilePath { get; init; }

    public string Read(int id)
    {
        var peopleLines = File.ReadAllLines("someFile.txt");
        return peopleLines.First(
            line => line.StartsWith(id.ToString()));
    }

    public void Write(int id, string firstName, string lastName)
    {
        var newLine = $"{id} {firstName} {lastName}";
        File.AppendAllLines(FilePath, new[] { newLine });
    }
}

public class CensusPersonalDataAccess_BreakingIsp : IPersonalDataAccess
{
    public string Read(int id)
    {

        var url = $"https://someCensusApi.gov/residents/{id}";

        var response = new HttpClient().GetStringAsync(url).Result;

        return JsonSerializer.Deserialize<CensusRecord>(response).PersonalData;
    }

    // This class cannot write to a census database, yet it is forced to provide an
    // implementation of the Write method. It provides a stub implementation that will
    // throw an exception when used.
    public void Write(int id, string firstName, string lastName)
    {
        throw new NotImplementedException();
    }
}

// One solution is to split the reading and writing between two interfaces.
// This way, TextFilePersonalDataAccess can implement both, and 
// CensusPersonalDataAccess will only implement IPersonalDataReader.

public interface IPersonalDataReader
{
    string Read(int id);
}

public interface IPersonalDataWriter
{
    void Write(int id, string firstName, string lastName);
}

public class TextFilePersonalDataAccess : IPersonalDataReader, IPersonalDataWriter
{
    private const string FilePath = "someFile.txt";

    public string Read(int id)
    {
        var peopleLines = File.ReadAllLines(FilePath);
        return peopleLines.First(
            line => line.StartsWith(id.ToString()));
    }

    public void Write(int id, string firstName, string lastName)
    {
        var newLine = $"{id} {firstName} {lastName}";
        File.AppendAllLines(FilePath, new[] { newLine });
    }
}

public class CensusPersonalDataAccess : IPersonalDataReader
{
    private const string Url = "censusData.com";

    public string Read(int id)
    {
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = client.GetAsync(Url).Result;
        response.EnsureSuccessStatusCode();
        string stringResponse = response.Content.ReadAsStringAsync().Result;
        var peopleLines = stringResponse.Split(Environment.NewLine);
        return peopleLines.First(line => line.StartsWith(id.ToString()));
    }
}

public class PersonalDataPrinter
{
    private IPersonalDataReader _personalDataReader;

    public PersonalDataPrinter(IPersonalDataReader personalDataReader)
    {
        _personalDataReader = personalDataReader;
    }

    public void Print(int id)
    {
        var personalData = _personalDataReader.Read(id);
        Console.WriteLine($"Data of the person with id {id}:");
        Console.WriteLine(personalData);
    }
}


// ##################
// helper types below

public record CensusRecord
{
    public string PersonalData { get; init; }
}
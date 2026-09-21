Console.WriteLine("Press any key to close.");
Console.ReadKey();

// ### Maintaining consistency within classes 

// This class is not consitent - it handles "not found" case differently in each case.
public class PersonalDataAccess_Inconsistent
{
    private readonly IPeopleRepository _peopleRepository;

    public PersonalDataAccess_Inconsistent(IPeopleRepository peopleRepository)
    {
        _peopleRepository = peopleRepository;
    }

    public Person GetById(int id)
    {
        var person = _peopleRepository.GetById(id);
        if (person == null)
        {
            throw new KeyNotFoundException($"No person with id {id} was found.");
        }
        return person;
    }

    public Person GetByNames(string firstName, string lastName)
    {
        var person = _peopleRepository.GetByNames(firstName, lastName);
        if (person == null)
        {
            Console.WriteLine("Person not found.");
        }
        return person;
    }

    public bool GetByDateOfBirth(DateTime dateOfBirth, out Person result)
    {
        result = _peopleRepository.GetByDateOfBirth(dateOfBirth);
        return result != null;
    }
}

// It's better to make each method behave consistently:
public class PersonalDataAccess
{
    private readonly IPeopleRepository _peopleRepository;

    public PersonalDataAccess(IPeopleRepository peopleRepository)
    {
        _peopleRepository = peopleRepository;
    }

    public Person GetById(int id)
    {
        var person = _peopleRepository.GetById(id);
        if (person == null)
        {
            throw new KeyNotFoundException(
                $"No person with id {id} was found.");
        }
        return person;
    }

    public Person GetByNames(string firstName, string lastName)
    {
        var person = _peopleRepository.GetByNames(firstName, lastName);
        if (person == null)
        {
            throw new KeyNotFoundException(
                $"No person named {firstName} {lastName} was found.");
        }
        return person;
    }

    public Person GetByDateOfBirth(DateTime dateOfBirth)
    {
        var person = _peopleRepository.GetByDateOfBirth(dateOfBirth);
        if (person == null)
        {
            throw new KeyNotFoundException(
                $"No person born on {dateOfBirth.ToShortDateString()} was found.");
        }
        return person;
    }
}


// ##################
// helper types below

public record Person
{

}

public interface IPeopleRepository
{
    Person GetById(int id);
    Person GetByDateOfBirth(DateTime dateOfBirth);
    Person GetByNames(string firstName, string lastName);
}

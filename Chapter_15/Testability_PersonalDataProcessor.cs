// This class is testable - it follows DIP and uses dependency inversion.
class PersonalDataProcessor
{
    private IPeopleRepository _peopleRepository;

    public PersonalDataProcessor(IPeopleRepository peopleRepository)
    {
        _peopleRepository = peopleRepository;
    }

    public int FindMaxAge()
    {
        var allPeople = _peopleRepository.GetAll();

        return allPeople.Max(person => person.Age);
    }
}

// This class is not testable, because it uses static methods.
class PersonalDataProcessor_Static
{
    public int FindMaxAge()
    {
        var allPeople = PeopleRepositoryStatic.GetAll();

        return allPeople.Max(person => person.Age);
    }
}

// This class is not testable, because it breaks the DIP.
class PersonalDataProcessor_BreakingDIP
{
    private PeopleRepository _peopleRepository;

    public PersonalDataProcessor_BreakingDIP(PeopleRepository peopleRepository)
    {
        _peopleRepository = peopleRepository;
    }

    public int FindMaxAge()
    {
        var allPeople = _peopleRepository.GetAll();

        return allPeople.Max(person => person.Age);
    }
}

// This class is not testable, because it does not use the Dependency Injection.
class PersonalDataProcessor_NoDependencyInjection
{
    private IPeopleRepository _peopleRepository;

    public PersonalDataProcessor_NoDependencyInjection()
    {
        _peopleRepository = new PeopleRepository();
    }

    public int FindMaxAge()
    {
        var allPeople = _peopleRepository.GetAll();

        return allPeople.Max(person => person.Age);
    }
}

public class PeopleRepositoryStatic
{
    public static List<Person> GetAll()
    {
        // Let's imagine this method actually connects to a database.
        return new List<Person>
            {
                new Person("Jake", "Smith", 45 ),
                new Person("Alexander", "Smith", 17 ),
                new Person("Beth", "Doe", 33)
            };
    }
}

public class PeopleRepository : IPeopleRepository
{
    public List<Person> GetAll()
    {
        // Let's imagine this method actually connects to a database.
        return new List<Person>
            {
                new Person("Holly", "Smith", 55 ),
                new Person("Ben", "Smith", 66 ),
                new Person("Anish", "Doe", 34)
            };
    }
}

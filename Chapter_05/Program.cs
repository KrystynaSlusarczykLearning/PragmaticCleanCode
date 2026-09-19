using Microsoft.Data.SqlClient;

var peopleDataReader = new PeopleDataReader("some connection string");
var people = peopleDataReader.GetAll();
var firstPerson = people.First();

var personalDataFormatter = new PersonalDataFormatter();
Console.WriteLine(
   "First person's data: " +
   personalDataFormatter.Format(firstPerson));

Console.WriteLine("Press any key to close.");
Console.ReadKey();

// Basics of Single Responsibility Principle

// This class breaks the Single Responsibility Principle
// because it both reads personal data and formats it.
public class PersonalDataAccess
{
    private readonly string _connectionString;

    public PersonalDataAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Person> GetAll()
    {
        var people = new List<Person>();

        using SqlConnection connection = new SqlConnection(_connectionString);

        connection.Open();
        string query = "SELECT Id, FirstName, LastName, Age FROM People";

        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            var person = new Person
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Age = reader.GetInt32(3)
            };
            people.Add(person);
        }

        return people;
    }

    public string FormatPersonalData(Person person)
    {
        return $"ID: {person.Id}, " +
            $"Name: {person.FirstName} {person.LastName}, " +
            $"Age: {person.Age}";
    }
}

//the two classes below implement the same functionality without breaking the SRP
public class PeopleDataReader
{
    private readonly string _connectionString;

    public PeopleDataReader(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Person> GetAll()
    {
        var people = new List<Person>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        {
            connection.Open();
            string query = "SELECT Id, FirstName, LastName, Age FROM People";

            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var person = new Person
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Age = reader.GetInt32(3)
                };
                people.Add(person);
            }
        }

        return people;
    }
}

public class PersonalDataFormatter
{
    public string Format(Person person)
    {
        return $"ID: {person.Id}, " +
            $"Name: {person.FirstName} {person.LastName}, " +
            $"Age: {person.Age}";
    }
}

// ### A single responsibility does not mean a single method 

// This class does not break SRP even though it has multiple methods.
// They all contribute to one responsibility: accessing and updating personal data.
public class PeopleRepository 
{
    public void AddPerson(Person person) { throw new NotImplementedException(); }
    public void UpdatePerson(Person person) { throw new NotImplementedException(); }
    public void DeletePerson(int personId) { throw new NotImplementedException(); }
    public Person GetPersonById(int personId) { throw new NotImplementedException(); }
    public IEnumerable<Person> GetAllPeople() { throw new NotImplementedException(); }
}

// ### Orchestration can still be one responsibility 

// The one responsibility of this class is to orchestrate the process of user authorization.
// It delegates particular steps to other types.
public class UserAuthorizer
{
    private readonly UsersRepository _usersRepository;
    private readonly ActionsRepository _actionsRepository;

    public UserAuthorizer(
        UsersRepository usersRepository,
        ActionsRepository actionsRepository)
    {
        _usersRepository = usersRepository;
        _actionsRepository = actionsRepository;
    }

    public bool IsAuthorized(int userId, ActionType actionType)
    {
        var user = _usersRepository.GetById(userId);
        var allowedActions = _actionsRepository.GetAllowedFor(user);

        return allowedActions.Any(action => action.Type == actionType);
    }
}


// ##################
// helper types below

public class Person
{
    public int Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public int Age { get; init; }
}

public class UsersRepository
{
    public User GetById(int userId)
    {
        throw new NotImplementedException();
    }
}

public class ActionsRepository
{
    public IEnumerable<Action> GetAllowedFor(User user)
    {
        throw new NotImplementedException();
    }
}

public enum ActionType
{

}

public record User
{

}

public record Action(ActionType Type);
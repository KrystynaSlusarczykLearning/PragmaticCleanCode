using System.Numerics;
using System.Text.RegularExpressions;

Console.WriteLine("Press any key to close.");
Console.ReadKey();

// ### Avoiding superficial ways of making code shorter 

// Longer code can often be more readable than shorter one.
// This implementation may be easier to follow...
int CalculateSumOfEvenNumbers_Longer(int n)
{
    int sum = 0;
    for (int i = 2; i <= n; i += 2)
    {
        sum += i;
    }

    return sum;
}

// ...than this one.
int CalculateSumOfEvenNumbers_Shorter(int n)
{
    return Enumerable.Range(2, n - 1)
        .Where(x => x % 2 == 0)
        .Sum();
}

// ### Compound names reveal multiple jobs 

// This class has "And" in its name,
// which suggests it's doing two things instead of one.
// It would be better to have two classes:
// one for confirming appointments, and one for notifying about visits.
public class AppointmentsConfirmerAndPatientNotificationsSender
{
    public void ConfirmAppointment(
        DateTime appointmentDate,
        Patient patient,
        Doctor doctor)
    {
        // Implementation 
    }

    public void NotifyAboutVisit(
        Patient patient,
        DateTime appointmentDate,
        Doctor doctor)
    {
        // Implementation 
    }
}

// ### Separating personal data from account data 

// This class mixes personal data of an user with account-related data.
public class UserAccount_TwoJobs
{
    private string _firstName;
    private string _lastName;
    private DateTime _dateOfBirth;

    private string _username;
    private string _password;
    private DateTime _lastLogin;

    public UserAccount_TwoJobs(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string username,
        string password,
        DateTime lastLogin)
    {
        _firstName = firstName;
        _lastName = lastName;
        _dateOfBirth = dateOfBirth;
        _username = username;
        _password = password;
        _lastLogin = lastLogin;
    }

    public string GetFullName()
    {
        return $"{_firstName} {_lastName}";
    }

    public int GetAge()
    {
        // Simplified for this example. 
        return DateTime.Now.Year - _dateOfBirth.Year;
    }

    public bool ValidateCredentials(string username, string password)
    {
        return _username == username && _password == password;
    }

    public void UpdateLastLogin()
    {
        _lastLogin = DateTime.Now;
    }
}

// It would be better to split it into two classes: Person and UserAccount
public class Person
{
    private string _firstName;
    private string _lastName;
    private DateTime _dateOfBirth;

    public Person(
        string firstName,
        string lastName,
        DateTime dateOfBirth)
    {
        _firstName = firstName;
        _lastName = lastName;
        _dateOfBirth = dateOfBirth;
    }

    public string GetFullName()
    {
        return $"{_firstName} {_lastName}";
    }

    public int GetAge()
    {
        // Simplified for this example. 
        return DateTime.Now.Year - _dateOfBirth.Year;
    }
}

public class UserAccount
{
    private string _username;
    private string _password;
    private DateTime _lastLogin;

    public UserAccount(
        string username,
        string password,
        DateTime lastLogin)
    {
        _username = username;
        _password = password;
        _lastLogin = lastLogin;
    }

    public bool ValidateCredentials(string username, string password)
    {
        return _username == username && _password == password;
    }

    public void UpdateLastLogin()
    {
        _lastLogin = DateTime.Now;
    }
}

// ### Grouping behavior that changes together 

// In this class, CalculateBilling is the part that will most likely change often,
// while other parts (basic customer data) will remain fixed.
// It would be a good idea to move the billing calculation to a separate type
// And keep customer's data in another type.
class CustomerService_TwoJobs
{
    private string _firstName;
    private string _lastName;
    private string _email;
    private string _phoneNumber;
    private List<Order> _orders;

    public CustomerService_TwoJobs(
        string firstName,
        string lastName,
        string email,
        string phoneNumber)
    {
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
        _phoneNumber = phoneNumber;
        _orders = new List<Order>();
    }

    public void AddOrder(Order order)
    {
        _orders.Add(order);
    }

    public void UpdatePhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
    }

    //changes happen frequently in this method
    //it would be better to have it in a separate class
    public decimal CalculateBilling()
    {
        decimal total = 0m;

        foreach (var order in _orders)
        {
            decimal orderTotal = order.Amount;

            if (order.Quantity > 10)
            {
                orderTotal *= 0.89m; // 11% discount for bulk orders
            }

            if (order.HasExpeditedShipping)
            {
                orderTotal += 15.0m; // Expedited shipping fee
            }

            decimal taxRate = GetTaxRate();
            orderTotal += orderTotal * taxRate;

            total += orderTotal;
        }

        // Apply customer loyalty discount
        if (_orders.Count > 4)
        {
            total *= 0.95m; // 5% loyalty discount
        }

        return total;
    }

    private decimal GetTaxRate()
    {
        // imagine some complex logic to determine tax rate 
        return 0.08m; // Example: 8% tax rate
    }
}

// The BillingCalculator class could then look like this:
class BillingCalculator
{
    public decimal Calculate(IEnumerable<Order> orders)
    {
        decimal total = 0m;

        foreach (var order in orders)
        {
            decimal orderTotal = order.Amount;

            if (order.Quantity > 10)
            {
                orderTotal *= 0.89m; // 11% discount for bulk orders
            }

            if (order.HasExpeditedShipping)
            {
                orderTotal += 15.0m; // Expedited shipping fee
            }

            decimal taxRate = GetTaxRate();
            orderTotal += orderTotal * taxRate;

            total += orderTotal;
        }

        // Apply customer loyalty discount
        if (orders.Count() > 4)
        {
            total *= 0.95m; // 5% loyalty discount
        }

        return total;
    }

    private decimal GetTaxRate()
    {
        // imagine some complex logic to determine tax rate 
        return 0.08m; // Example: 8% tax rate
    }
}

// Customer's data could now be stored in a dedicated type, independent of
// billing calculation.
public class Customer
{
    private string _firstName;
    private string _lastName;
    private string _email;
    private string _phoneNumber;
    private List<Order> _orders;

    public Customer(
        string firstName,
        string lastName,
        string email,
        string phoneNumber)
    {
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
        _phoneNumber = phoneNumber;
        _orders = new List<Order>();
    }

    public void AddOrder(Order order)
    {
        _orders.Add(order);
    }

    public void UpdatePhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
    }
}

// ### Separating levels of abstraction 

// Low-level string processing like in the Clean method
// does not belong in this high-level class.
// It should be moved to a dedicated class that can be reused elsewhere.
public class BlogPostsStorage
{
    private readonly Regex _regex = new Regex("[ ]{2,}", RegexOptions.None);

    private readonly IPostsRepository _postsRepository;

    public BlogPostsStorage(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }

    public void SaveBlogPost(string content, DateTime createdDate)
    {
        var cleanedContent = Clean(content);
        _postsRepository.Save(cleanedContent, createdDate);
    }

    private string Clean(string content)
    {
        return _regex.Replace(content, " ");
    }
}

// Now removing excessive spaces can easily be used elsewhere.
public static class StringExtensions
{
    private static readonly Regex Regex = new Regex("[ ]{2,}", RegexOptions.None);

    public static string RemoveExcessiveSpaces(this string content)
    {
        return Regex.Replace(content, " ");
    }
}

// ### Avoiding unnecessary class proliferation 

// Refactoring towards smaller classes gives us more types,
// but it makes the code more granular, easier to read, and easier to reuse.
public class AccountStorage
{
    private readonly ICredentialsValidator _credentialsValidator;
    private readonly IUsersRepository _usersRepository;
    private readonly IUserCommunication _userCommunication;

    public AccountStorage(
        ICredentialsValidator credentialsValidator,
        IUsersRepository usersRepository,
        IUserCommunication userCommunication)
    {
        _credentialsValidator = credentialsValidator;
        _usersRepository = usersRepository;
        _userCommunication = userCommunication;
    }

    // Following the logic of this method is easy, because
    // each step is delegated to a dedicated type and a well-named method.
    public void CreateNewAccount(string username, string password)
    {
        if (_credentialsValidator.IsValidUsername(username) &&
            _credentialsValidator.IsValidPassword(password))
        {
            _usersRepository.Save(username, password);
            _userCommunication.ShowMessage("Account created successfully.");
        }
        else
        {
            _userCommunication.ShowWarning("Invalid user credentials.");
        }
    }
}

// ##################
// helper types below

public record Patient
{

}

public record Doctor
{

}

public class Order
{
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
    public bool HasExpeditedShipping { get; set; }
}

public interface IPostsRepository
{
    void Save(string cleanedContent, DateTime createdDate);
}

public interface ICredentialsValidator
{
    bool IsValidPassword(string password);
    bool IsValidUsername(string username);
}

public interface IUsersRepository
{
    void Save(string username, string password);
}

public interface IUserCommunication
{
    void ShowMessage(string message);
    void ShowWarning(string message);
}

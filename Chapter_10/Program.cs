using Moq;
using NUnit.Framework;

Console.WriteLine("Press any key to close.");
Console.ReadKey();

public static class Math
{
    // This method uses no instance state, so it can be made static.
    // It's safe to do, because there is no reasonable use case when we would like to
    // switch this implementation to something else.
    public static int Min(int a, int b)
    {
        return a < b ? a : b;
    }
}

public class CustomList<T>
{
    private T[] _items = [];
    public int Count { get; private set; }

    //This method uses the instance state; it cannot be made static.
    public void Add(T item)
    {
        _items[Count] = item;
        ++Count;
    }
}

public class DataAccess : IDataAccess
{
    // It would be a bad idea to make this method static,
    // even if it doesn't use the instance state,
    // because then, we would not be able to switch its implementation to something else,
    // including a mock one for testing purposes.
    public List<int> GetData()
    {
        // connects to a DB and reads the data 
        return new List<int>();
    }
}

public interface IDataAccess
{
    List<int> GetData();
}

public class DataProcessor
{
    private readonly IDataAccess _dataAccess;

    public DataProcessor(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    // Thanks to GetData being non-static, we can mock it in tests or easily switch it
    // to another implementation.
    public int CalculateSum()
    {
        return _dataAccess.GetData().Sum();
    }
}

// ### Wrapping static framework APIs 
// This method will be hard to test,
// because static Now property cannot be mocked.
public class DateDescriptor
{
    string GetCurrentDayDescription()
    {
        DateTime today = DateTime.Now;
        return $"Today is {today.DayOfWeek}";
    }
}

// Solution: a wrapper class + interface.
// Then, the interface can be mocked.
interface IDateTime
{
    DateTime Now { get; }
}

class DateTimeWrapper : IDateTime
{
    public DateTime Now
    {
        get
        {
            return DateTime.Now;
        }
    }
}

// ### Keeping the dependency graph visible 

// In this code, the dependency graph is obscured
// BankAccount needs TransferServiceManager and InternalServiceQueue
// yet this dependency is hidden behind Initialize methods
public class BankAccountTests_DependencyGraphObscured
{
    [Test]
    public void TransferTo_ShallRemoveAmountFromOneAccount_AndAddItToAnother()
    {
        TransferServiceManager_DependencyGraphObscured.Initialize();
        InternalServiceQueue_DependencyGraphObscured.Initialize();

        var bankAccount1 = new BankAccount_DependencyGraphObscured();
        bankAccount1.AddFunds(1000);

        var bankAccount2 = new BankAccount_DependencyGraphObscured();
        bankAccount1.TransferTo(bankAccount2, 200);

        Assert.That(bankAccount1.Balance, Is.EqualTo(800));
        Assert.That(bankAccount2.Balance, Is.EqualTo(200));
    }
}

public class TransferServiceManager_DependencyGraphObscured
{
    public static void Initialize()
    {
        throw new NotImplementedException();
    }
}

public class InternalServiceQueue_DependencyGraphObscured
{
    internal static void Initialize()
    {
        throw new NotImplementedException();
    }
}

class BankAccount_DependencyGraphObscured
{
    public int Balance { get; private set; }

    public void AddFunds(int amount)
    {
        // imagine a lot of complicated code
        throw new NotImplementedException();
    }

    public void TransferTo(BankAccount_DependencyGraphObscured other, int amount)
    {
        // imagine a lot of complicated code
        throw new NotImplementedException();
    }
}

// The solution is to make the dependencies clearly visible.
// Then, one does not need to know that the Initialize methods must be called
// before BankAccount is used.
[TestFixture]
public class BankAccountTests
{
    [Test]
    public void TransferTo_ShallRemoveAmountFromOneAccount_AndAddItToAnother()
    {
        var bankAccount1 = new BankAccount(
            new Mock<ITransferServiceManager>().Object);
        bankAccount1.AddFunds(1000);

        var bankAccount2 = new BankAccount(
            new Mock<ITransferServiceManager>().Object);
        bankAccount1.TransferTo(bankAccount2, 200);

        Assert.That(bankAccount1.Balance, Is.EqualTo(800));
        Assert.That(bankAccount2.Balance, Is.EqualTo(200));
    }
}

class BankAccount
{
    private ITransferServiceManager _transferServiceManager;

    public BankAccount(ITransferServiceManager transferServiceManager)
    {
        _transferServiceManager = transferServiceManager;
    }

    public int Balance { get; private set; }

    public void AddFunds(int amount)
    {
        // imagine a lot of complicated code
        throw new NotImplementedException();
    }

    public void TransferTo(BankAccount other, int amount)
    {
        // imagine a lot of complicated code
        throw new NotImplementedException();
    }
}


public interface ITransferServiceManager
{

}

public interface IInternalServiceQueue
{

}

class TransferServiceManager : ITransferServiceManager
{
    private IInternalServiceQueue _internalServiceQueue;

    public TransferServiceManager(IInternalServiceQueue internalServiceQueue)
    {
        _internalServiceQueue = internalServiceQueue;
    }
}

class InternalServiceQueue : IInternalServiceQueue
{
}
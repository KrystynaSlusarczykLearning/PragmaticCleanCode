namespace Chapter_07.AvoidingStrengthenedPreconditions;

// This design breaks the LSP, because ChildrenAccount strenghtens the precondition
// defined by its base type.
public class BankAccount_BreakingLsp
{
    public virtual void Withdraw(int amount)
    {
        if (amount < 10000)
        {
            Console.WriteLine($"Withdrawing amount: {amount}");
            //perform withdrawal
        }
        else
        {
            Console.WriteLine(
                $"Withdrawing {amount} requires extra authorization");
            //do not perform withdrawal
        }
    }
}

public class ChildrenAccount : BankAccount_BreakingLsp
{
    public override void Withdraw(int amount)
    {
        if (amount < 1000)
        {
            Console.WriteLine($"Withdrawing amount: {amount}");
            //perform withdrawal
        }
        else
        {
            Console.WriteLine(
                $"Withdrawing {amount} requires extra authorization");
            //do not perform withdrawal
        }
    }
}

// One solution is to keep one BankAccount type and supply the threshold as configuration.

public enum BankAccountType
{
    Adults,
    Children
}

public class BankAccount
{
    private int _maxWithdrawalWithoutExtraAuthorization;

    private BankAccount(int maxWithdrawalWithoutExtraAuthorization)
    {
        _maxWithdrawalWithoutExtraAuthorization = maxWithdrawalWithoutExtraAuthorization;
    }

    public static BankAccount Create(BankAccountType type)
    {
        switch (type)
        {
            case BankAccountType.Adults:
                return new BankAccount(10000);
            case BankAccountType.Children:
                return new BankAccount(1000);
        }
        throw new NotSupportedException("Unsupported account type.");
    }

    public virtual void Withdraw(int amount)
    {
        if (amount < _maxWithdrawalWithoutExtraAuthorization)
        {
            Console.WriteLine($"Withdrawing amount: {amount}");
            //perform withdrawal
        }
        else
        {
            Console.WriteLine(
                $"Withdrawing {amount} requires extra authorization");
            //do not perform withdrawal
        }
    }
}


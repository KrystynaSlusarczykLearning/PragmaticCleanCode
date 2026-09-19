var videosRepository = new VideosRepository();

// Each User type knows which videos are available for them.
var user = new User(new FreeAccount(videosRepository));
Console.WriteLine("Videos this user can see: " +
    string.Join(", ", user.GetAvailableVideos()));

var youngUser = new User(new KidsAccount(videosRepository));

var availableVideos = youngUser.GetAvailableVideos();

Console.WriteLine("Videos this young user can see: " +
    string.Join(", ", availableVideos));

// Factory knows how to create a user based on the AccountType.
AccountType accountType = AccountType.Free; //value chosen by the user
var newUser = new User(new AccountsFactory().CreateAccount(accountType));

Console.WriteLine("Press any key to close.");
Console.ReadKey();

// This class breaks the OCP - adding the Kids account forces us to 
// modify the GetAvailableVideos method.
public class User_BreakingOcp
{
    public AccountType AccountType { get; }
    private readonly IVideosRepository _videosRepository;

    public User_BreakingOcp(AccountType accountType, IVideosRepository videosRepository)
    {
        AccountType = accountType;
        _videosRepository = videosRepository;
    }

    public List<Video> GetAvailableVideos()
    {
        if (AccountType == AccountType.Premium)
        {
            return _videosRepository.GetAll();
        }

        if (AccountType == AccountType.Kids)
        {
            return _videosRepository
                .GetAll()
                .Where(video => video.IsForKids)
                .ToList();
        }

        return _videosRepository
            .GetAll()
            .Where(video => video.IsFree)
            .ToList();
    }
}

// This design does not break the OCP - the scope of change when a new account type is
// introduced is contained, and does not affect the User class.
public class User
{
    private readonly IAccount _account;

    public User(IAccount account)
    {
        _account = account;
    }

    public List<Video> GetAvailableVideos()
    {
        return _account.GetAvailableVideos();
    }
}

public interface IAccount
{
    AccountType AccountType { get; }
    List<Video> GetAvailableVideos();
}

// Adding new account type only requires defining a new type implementing IAccount.
public class KidsAccount : IAccount
{
    public AccountType AccountType { get; } = AccountType.Kids;
    private IVideosRepository _videosRepository;

    public KidsAccount(IVideosRepository videosRepository)
    {
        _videosRepository = videosRepository;
    }

    public List<Video> GetAvailableVideos()
    {
        return _videosRepository
            .GetAll()
            .Where(video => video.IsForKids)
            .ToList();
    }
}

public class FreeAccount : IAccount
{
    public AccountType AccountType { get; } = AccountType.Free;
    private IVideosRepository _videosRepository;

    public FreeAccount(IVideosRepository videosRepository)
    {
        _videosRepository = videosRepository;
    }

    public List<Video> GetAvailableVideos()
    {
        return _videosRepository
            .GetAll()
            .Where(video => video.IsFree)
            .ToList();
    }
}

public class PremiumAccount : IAccount
{
    public AccountType AccountType { get; } = AccountType.Premium;
    private IVideosRepository _videosRepository;

    public PremiumAccount(IVideosRepository videosRepository)
    {
        _videosRepository = videosRepository;
    }

    public List<Video> GetAvailableVideos()
    {
        return _videosRepository
            .GetAll();
    }
}

// The selection logic is contained within a factory.
public class AccountsFactory
{
    public IAccount CreateAccount(AccountType accountType)
    {
        var videosRepository = new VideosRepository();

        switch (accountType)
        {
            case AccountType.Free:
                return new FreeAccount(videosRepository);
            case AccountType.Kids:
                return new KidsAccount(videosRepository);
            case AccountType.Premium:
                return new PremiumAccount(videosRepository);
        }

        throw new NotSupportedException("Invalid account type");
    }
}


// ##################
// helper types below

public enum AccountType
{
    Free,
    Premium,
    Kids
}

public class Video
{
    public bool IsForKids { get; init; }
    public bool IsFree { get; init; }
    public string Title { get; init; }

    public override string ToString() => Title;
}

public interface IVideosRepository
{
    List<Video> GetAll();
    List<Video> GetAll(string region);
}

public class VideosRepository : IVideosRepository
{
    public List<Video> GetAll()
    {
        return new List<Video>
        {
            new Video { IsFree = true, Title = "City Lights" },
            new Video { IsFree = false, Title = "The Kid" },
            new Video { IsFree = false, IsForKids = true, Title = "The Lion King" },
        };
    }

    public List<Video> GetAll(string region)
    {
        throw new NotImplementedException();
    }
}
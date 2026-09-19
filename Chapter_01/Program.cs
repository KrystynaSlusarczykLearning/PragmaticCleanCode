using System.Runtime.Serialization;

Console.WriteLine("Press any key to close.");
Console.ReadKey();

public class Authorizer
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPermissionsRepository _permissionsRepository;

    public Authorizer(
        IUsersRepository usersRepository,
        IPermissionsRepository permissionsRepository)
    {
        _usersRepository = usersRepository;
        _permissionsRepository = permissionsRepository;
    }

    //hard-to-read version
    public AuthorizationResult Auth(
     User u, UserAction ua)
    {
        if (_usersRepository
            .GetById(u.Id) == null)
        {
            throw new NonExistentUserException(u);
        }
        var perm = _permissionsRepository
            .GetForUser(u.Id);
        if (u.IsSuperUser)
        {
            return AuthorizationResult
            .Authorized;
        }
        foreach (var p in perm)
        {
            if (p.Action == ua)
            {
                return AuthorizationResult
                .Authorized;
            }
        }
        return AuthorizationResult
            .NotAuthorized;
    }

    //clean version
    public AuthorizationResult Authorize(
        User user, UserAction action)
    {
        if (!_usersRepository.DoesExist(user))
        {
            throw new NonExistentUserException(user);
        }

        if (user.IsSuperUser ||
            IsUserAuthorizedToPerform(action, user))
        {
            return AuthorizationResult.Authorized;
        }

        return AuthorizationResult.NotAuthorized;
    }

    private bool IsUserAuthorizedToPerform(
        UserAction action, User user)
    {
        var userPermissions = _permissionsRepository.GetForUser(user.Id);

        return userPermissions.Any(
            permission => permission.Action == action);
    }
}

// ##################
// helper types below

public enum AuthorizationResult
{
    Authorized,
    NotAuthorized
}

public enum UserAction
{

}

public class User
{
    public int Id { get; init; }
    public bool IsSuperUser { get; init; }
}

public class Permission
{
    public UserAction Action { get; init; }
}

public interface IUsersRepository
{
    bool DoesExist(User user);
    User GetById(int id);
}

public interface IPermissionsRepository
{
    IEnumerable<Permission> GetForUser(int id);
}

public class NonExistentUserException : Exception
{
    public NonExistentUserException()
    {
    }

    public NonExistentUserException(User user) : base("User does not exist.")
    {
    }

    public NonExistentUserException(string? message) : base(message)
    {
    }

    public NonExistentUserException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NonExistentUserException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
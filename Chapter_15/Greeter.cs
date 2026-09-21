public class Greeter
{
    private IUserCommunication _userCommunication;

    public Greeter(IUserCommunication userCommunication)
    {
        _userCommunication = userCommunication;
    }

    public void Greet(string name)
    {
        _userCommunication.ShowMessage($"Hello, {name}!");
    }
}

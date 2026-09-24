public class ConsoleUserCommunication : IUserCommunication
{
    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }
}

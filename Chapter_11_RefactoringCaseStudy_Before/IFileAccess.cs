public interface IFileAccess
{
    IEnumerable<string> ReadAllLines(string path);
}

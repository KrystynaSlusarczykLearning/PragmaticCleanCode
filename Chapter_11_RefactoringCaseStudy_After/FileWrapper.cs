public class FileWrapper : IFileAccess
{
    public IEnumerable<string> ReadAllLines(string path)
    {
        return File.ReadAllLines(path);
    }
}
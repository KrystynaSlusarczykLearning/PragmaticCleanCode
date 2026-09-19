Employee employee = new Developer
{
    FirstName = "Anna",
    LastName = "Lima",
    Position = "Developer",
    HoursThisMonth = 160,
    BugsFixed = 6
};

var employeeReportCreator = new EmployeeReportCreator(new FileWrapper());
employeeReportCreator.CreateReportFor(employee);

Console.WriteLine("Done!");
Console.ReadKey();

public interface IFile
{
    void WriteAllText(string path, string contents);
}

public class FileWrapper : IFile
{
    public void WriteAllText(string path, string contents)
    {
        File.WriteAllText(path, contents);
    }
}

public interface IEmployeeReportCreator
{
    void CreateReportFor(Employee employee);
}

public class EmployeeReportCreator : IEmployeeReportCreator
{
    private IFile _file;

    public EmployeeReportCreator(IFile file)
    {
        _file = file;
    }

    public void CreateReportFor(Employee employee)
    {
        var report = employee.GenerateReport();

        SaveToFile(BuildFileName(employee), report);
    }

    private void SaveToFile(string path, string contents)
    {
        _file.WriteAllText(path, contents);
    }

    private string BuildFileName(Employee employee)
    {
        return $"{employee.FirstName}_{employee.LastName}.txt";
    }
}

public class Employee
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Position { get; init; }
    public int HoursThisMonth { get; init; }

    public virtual string GenerateReport()
    {
        return
            $"{FirstName} {LastName}," +
            $" working as {Position}," +
            $" worked {HoursThisMonth} hours this month.";
    }
}

public class Developer : Employee
{
    public int BugsFixed { get; init; }

    public override string GenerateReport()
    {
        return
            base.GenerateReport().TrimEnd('.') +
            $" and fixed {BugsFixed} bugs.";
    }
}

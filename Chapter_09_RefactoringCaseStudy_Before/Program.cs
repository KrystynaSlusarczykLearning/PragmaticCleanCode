Employee employee = new Developer
{
    FirstName = "Anna",
    LastName = "Lima",
    Position = "Developer",
    HoursThisMonth = 160,
    BugsFixed = 6
};

var employeeReportCreator = new EmployeeReportCreator();
employeeReportCreator.CreateReportFor(employee);

Console.WriteLine("Done! The result was saved to the output folder: " + 
    Environment.NewLine + 
    AppContext.BaseDirectory);

Console.ReadKey();

public interface IEmployeeReportCreator
{
    void CreateReportFor(Employee employee);
    void SaveToFile(string path, string contents);
}

public class EmployeeReportCreator : IEmployeeReportCreator
{
    public void CreateReportFor(Employee employee)
    {
        ReportGenerator reportGenerator;

        if (employee.Position == "Developer")
        {
            reportGenerator = new DeveloperReportGenerator();
        }
        else
        {
            reportGenerator = new ReportGenerator();
        }

        var report = reportGenerator.Generate(employee);

        SaveToFile(BuildFileName(employee), report);
    }

    public void SaveToFile(string path, string contents)
    {
        File.WriteAllText(path, contents);
    }

    private string BuildFileName(Employee employee)
    {
        return $"{employee.FirstName}_{employee.LastName}.txt";
    }
}

public class ReportGenerator
{
    public virtual string Generate(Employee employee)
    {
        return
            $"{employee.FirstName} {employee.LastName}," +
            $" working as {employee.Position}," +
            $" worked {employee.HoursThisMonth} hours this month.";
    }
}

public class DeveloperReportGenerator : ReportGenerator
{
    public override string Generate(Employee employee)
    {
        if (employee.Position != "Developer")
        {
            throw new InvalidOperationException(
                "This report can only be generated for developers");
        }

        return
            $"{employee.FirstName} {employee.LastName}," +
            $" working as {employee.Position}," +
            $" worked {employee.HoursThisMonth} hours this month" +
            $" and fixed {(employee as Developer).BugsFixed} bugs.";
    }
}

public class Employee
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Position { get; init; }
    public int HoursThisMonth { get; init; }
}

public class Developer : Employee
{
    public int BugsFixed { get; init; }
}


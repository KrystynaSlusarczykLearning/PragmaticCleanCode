public class VacationPlan
{
    public DateTime Start { get; }
    public int TotalDays { get; }

    public VacationPlan(DateTime start, int totalDays)
    {
        Start = start;
        TotalDays = totalDays;
    }
}

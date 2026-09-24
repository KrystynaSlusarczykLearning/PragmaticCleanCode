public class VacationPlanPrinter
{
    private IUserCommunication _userCommunication;

    public VacationPlanPrinter(IUserCommunication userCommunication)
    {
        _userCommunication = userCommunication;
    }

    public void ShowReport(VacationPlan vacationPlan, int vacationDaysToUse)
    {
        var startDate = vacationPlan.Start.ToShortDateString();

        var endDate =
            vacationPlan
            .Start
            .AddDays(vacationPlan.TotalDays - 1)
            .ToShortDateString();

        _userCommunication.ShowMessage(
           $"Vacation days used: {vacationDaysToUse}\n" +
           $"Optimal start date: {startDate}\n" +
           $"Optimal end date: {endDate}\n" +
           $"Total vacation days: {vacationPlan.TotalDays}\n");
    }
}

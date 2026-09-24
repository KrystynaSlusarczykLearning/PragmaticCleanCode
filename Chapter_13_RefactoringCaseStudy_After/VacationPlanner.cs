public class VacationPlanner
{
    private IPublicHolidaysProvider _publicHolidaysProvider;

    public VacationPlanner(IPublicHolidaysProvider publicHolidaysProvider)
    {
        _publicHolidaysProvider = publicHolidaysProvider;
    }

    public VacationPlan FindOptimalVacation(int vacationDaysToUse, int year)
    {
        var startDate = new DateTime(year, 1, 1);
        var endDate = new DateTime(year, 12, 31);

        DateTime bestStartDate = startDate;
        int maxVacationLength = 0;

        var publicHolidays = _publicHolidaysProvider.GetFor(year);

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            int vacationLength = CalculateVacationLength(
                date, vacationDaysToUse, publicHolidays);
            if (vacationLength > maxVacationLength)
            {
                bestStartDate = date;
                maxVacationLength = vacationLength;
            }
        }

        return new VacationPlan(bestStartDate, maxVacationLength);
    }

    private int CalculateVacationLength(
        DateTime vacationStart,
        int vacationDays,
        IEnumerable<DateTime> publicHolidays)
    {
        int usedVacationDays = 0;
        int totalVacationLength = 0;
        DateTime day = vacationStart;

        while (usedVacationDays < vacationDays)
        {
            if (_publicHolidaysProvider.IsWeekend(day) ||
                publicHolidays.Contains(day))
            {
                totalVacationLength++;
            }
            else
            {
                totalVacationLength++;
                usedVacationDays++;
            }

            day = day.AddDays(1);
        }

        // Keep adding weekend/public holiday days after vacation ends
        while (_publicHolidaysProvider.IsWeekend(day) ||
               publicHolidays.Contains(day))
        {
            totalVacationLength++;
            day = day.AddDays(1);
        }

        return totalVacationLength;
    }
}
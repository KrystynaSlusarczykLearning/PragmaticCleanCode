public class VacationPlanner
{
    public VacationPlan FindOptimalVacation(int vacationDaysToUse, int year)
    {
        var startDate = new DateTime(year, 1, 1);
        var endDate = new DateTime(year, 12, 31);

        DateTime bestStartDate = startDate;
        int maxHolidaysLength = 0;

        var publicHolidays = new PolishPublicHolidaysProvider().GetFor(year);

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            int vacationLength = CalculateHolidayLength(date, vacationDaysToUse, publicHolidays);
            if (vacationLength > maxHolidaysLength)
            {
                bestStartDate = date;
                maxHolidaysLength = vacationLength;
            }
        }

        return new VacationPlan(bestStartDate, maxHolidaysLength);
    }

    private int CalculateHolidayLength(
        DateTime vacationStart,
        int vacationDays,
        List<DateTime> publicHolidays)
    {
        int usedVacationDays = 0;
        int totalVacationLength = 0;
        DateTime day = vacationStart;

        while (usedVacationDays < vacationDays)
        {
            if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday ||
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
        while (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday ||
            publicHolidays.Contains(day))
        {
            totalVacationLength++;
            day = day.AddDays(1);
        }

        return totalVacationLength;
    }

    public void ShowReport(VacationPlan vacationPlan, int vacationDaysToUse)
    {
        Console.WriteLine(
           $"Vacation days used: {vacationDaysToUse}\n" +
           $"Optimal start date: {vacationPlan.Start.ToShortDateString()}\n" +
           $"Optimal end date: {vacationPlan.Start.AddDays(vacationPlan.TotalDays - 1).ToShortDateString()}\n" +
           $"Total vacation days: {vacationPlan.TotalDays}\n");
    }
}

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

public class PolishPublicHolidaysProvider
{
    public List<DateTime> GetFor(int year)
    {
        return new List<DateTime>
        {
            new DateTime(year, 1, 1),   // New Year's Day
            new DateTime(year, 1, 6),  // Epiphany
            GetEasterSunday(year).AddDays(1),  //Easter Monday
            new DateTime(year, 5, 1),   // Labour Day
            new DateTime(year, 5, 3),// Constitution Day
            GetCorpusChristi(year),     // Corpus Christi
            new DateTime(year, 8, 15),  // Assumption of Mary
            new DateTime(year, 11, 1),  // All Saints' Day
            new DateTime(year, 11, 11),// Independence Day
            new DateTime(year, 12, 25), // Christmas Day
            new DateTime(year, 12, 26), // Boxing Day
        };
    }

    private static DateTime GetEasterSunday(int year)
    {
        // Calculation for Easter Sunday using the Computus algorithm
        int lunarFactor = year % 19;
        int century = year / 100;
        int centuryRemainder = year % 100;
        int leapYearsFactor = century / 4;
        int leapYearsReminder = century % 4;
        int calendarCorrectionsFactor = (century + 8) / 25;
        int lunarCycleAdjustment = (century - calendarCorrectionsFactor + 1) / 3;
        int epact = (19 * lunarFactor + century - leapYearsFactor - lunarCycleAdjustment + 15) % 30;
        int leapYearsAdjustment = centuryRemainder / 4;
        int leapYearsAdjustmentRemainder = centuryRemainder % 4;
        int fullMoonPositionFactor = (32 + 2 * leapYearsReminder + 2 * leapYearsAdjustment - epact - leapYearsAdjustmentRemainder) % 7;
        int monthFactor = (lunarFactor + 11 * epact + 22 * fullMoonPositionFactor) / 451;
        int month = (epact + fullMoonPositionFactor - 7 * monthFactor + 114) / 31;
        int day = ((epact + fullMoonPositionFactor - 7 * monthFactor + 114) % 31) + 1;

        return new DateTime(year, month, day);
    }

    private static DateTime GetCorpusChristi(int year)
    {
        // Corpus Christi is 60 days after Easter
        return GetEasterSunday(year).AddDays(60);
    }
}
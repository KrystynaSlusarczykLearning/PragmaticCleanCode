public class PolishPublicHolidaysProvider : IPublicHolidaysProvider
{
    public bool IsWeekend(DateTime day)
    {
        return day.DayOfWeek == DayOfWeek.Saturday ||
               day.DayOfWeek == DayOfWeek.Sunday;
    }

    public IEnumerable<DateTime> GetFor(int year)
    {
        return new List<DateTime>
        {
            new DateTime(year, 1, 1),          // New Year's Day
            new DateTime(year, 1, 6),          // Epiphany
            GetEasterSunday(year).AddDays(1),  // Easter Monday
            new DateTime(year, 5, 1),          // Labour Day
            new DateTime(year, 5, 3),          // Constitution Day
            GetCorpusChristi(year),            // Corpus Christi
            new DateTime(year, 8, 15),         // Assumption of Mary
            new DateTime(year, 11, 1),         // All Saints' Day
            new DateTime(year, 11, 11),        // Independence Day
            new DateTime(year, 12, 25),        // Christmas Day
            new DateTime(year, 12, 26),        // Boxing Day
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
        int epact = (19 * lunarFactor + century - 
            leapYearsFactor - lunarCycleAdjustment + 15) % 30;
        int leapYearsAdjustment = centuryRemainder / 4;
        int leapYearsAdjustmentRemainder = centuryRemainder % 4;
        int fullMoonPositionFactor = 
            (32 + 2 * leapYearsReminder + 2 * leapYearsAdjustment 
            - epact - leapYearsAdjustmentRemainder) % 7;
        int monthFactor = (lunarFactor + 11 * epact + 
            22 * fullMoonPositionFactor) / 451;
        int month = (epact + fullMoonPositionFactor - 
            7 * monthFactor + 114) / 31;
        int day = ((epact + fullMoonPositionFactor - 
            7 * monthFactor + 114) % 31) + 1;

        return new DateTime(year, month, day);
    }

    private static DateTime GetCorpusChristi(int year)
    {
        // Corpus Christi is 60 days after Easter
        return GetEasterSunday(year).AddDays(60);
    }
}
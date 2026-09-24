public interface IPublicHolidaysProvider
{
    IEnumerable<DateTime> GetFor(int year);
    bool IsWeekend(DateTime day);
}

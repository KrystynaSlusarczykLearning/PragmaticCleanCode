class DateRandomizer
{
    private IRandom _random;

    public DateRandomizer(IRandom random)
    {
        _random = random;
    }

    public DateTime GenerateFor(int year)
    {
        DateTime startOfYear = new DateTime(year, 1, 1);
        int daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
        int randomDayOffset = _random.Next(daysInYear);

        return startOfYear.AddDays(randomDayOffset);
    }
}

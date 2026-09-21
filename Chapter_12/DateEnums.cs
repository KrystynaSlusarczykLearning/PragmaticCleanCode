// These enums are kept in one file on purpose.
// Usually each type should live in its own file, named after the type.
// Here the enums are small, closely related (they all describe the calendar),
// and unlikely to change, so keeping them together makes them easier to find
// and to read as a whole.
// If any of them grows or starts carrying logic, move it to its own file.

namespace PragmaticCleanCode.Calendar;

public enum Month
{
    January = 1,
    February = 2,
    March = 3,
    April = 4,
    May = 5,
    June = 6,
    July = 7,
    August = 8,
    September = 9,
    October = 10,
    November = 11,
    December = 12
}

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter
}

public enum Quarter
{
    Q1 = 1,
    Q2 = 2,
    Q3 = 3,
    Q4 = 4
}

public enum PartOfDay
{
    Morning,
    Afternoon,
    Evening,
    Night
}
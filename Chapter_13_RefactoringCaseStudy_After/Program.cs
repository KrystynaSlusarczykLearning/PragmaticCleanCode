VacationPlanner planner = new VacationPlanner(
    new PolishPublicHolidaysProvider());

int vacationDaysToUse = 5;

var optimalVacation = planner.FindOptimalVacation(
    vacationDaysToUse, 2027);

var vacationPlanPrinter = new VacationPlanPrinter(
    new ConsoleUserCommunication());
vacationPlanPrinter.ShowReport(optimalVacation, vacationDaysToUse);

Console.WriteLine("Press any key to close.");
Console.ReadKey();

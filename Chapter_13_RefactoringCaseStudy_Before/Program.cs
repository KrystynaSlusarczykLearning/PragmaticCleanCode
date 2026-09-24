VacationPlanner planner = new VacationPlanner();

int vacationDaysToUse = 5;

var optimalVacation = planner.FindOptimalVacation(vacationDaysToUse, 2027);
planner.ShowReport(optimalVacation, vacationDaysToUse);

Console.WriteLine("Press any key to close.");
Console.ReadKey();

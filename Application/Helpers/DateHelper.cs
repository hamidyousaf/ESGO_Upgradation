namespace Application.Helpers;

internal static class DateHelper
{
    public static int GetTotalDays(DateTime startDate, DateTime endDate)
    {
        TimeSpan difference = endDate - startDate;
        if (difference.Days <= 0)
        {
            return 1;
        }

        return difference.Days + 1;
    }
    public static double GetTotalMinutes(TimeOnly startTime, TimeOnly endTime)
    {
        TimeSpan difference = endTime - startTime;
        return difference.TotalMinutes;
    }
    public static double GetTotalHours(TimeOnly startTime, TimeOnly endTime)
    {
        TimeSpan difference = endTime - startTime;
        return difference.TotalHours;
    }
    public static decimal ConvertToDecimal(TimeOnly time)
    {
        // Calculate the decimal representation of hours and minutes
        decimal decimalHours = time.Hour + (time.Minute / 60.0m);
        return decimalHours;
    }
}

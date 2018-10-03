using System;

namespace WebdriverTestProject.Helpers
{
    public static class NextDayOfWeek
    {
        public static DateTime GetNextDayOfWeek(this DateTime date, DayOfWeek day)
        {
            var diff = ((int) day - (int) date.DayOfWeek + 6) % 7;
            return date.AddDays(diff + 1);
        }
    }
}
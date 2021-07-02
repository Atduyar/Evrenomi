using System;
using Business.Constants;
using Business.Extensions;

namespace Business.Helpers
{
    public class DateTimeBasicHelper: IDateTimeHelper
    {
        public string SetTime(DateTime dateTime)
        {
            TimeSpan passingTime = DateTime.Now.Subtract(dateTime);
            if (passingTime.GetYears() > 0)
            {
                return $"{passingTime.GetYears()} {Messages.DateTimePassingYear}";
            }
            else if (passingTime.GetMonths() > 0)
            {
                return $"{passingTime.GetMonths()} {Messages.DateTimePassingMonths}";
            }
            else if (passingTime.Days > 0)
            {
                return $"{passingTime.Days} {Messages.DateTimePassingDays}";
            }
            else if (passingTime.Hours > 0)
            {
                return $"{passingTime.Hours} {Messages.DateTimePassingHours}";
            }
            else if (passingTime.Minutes > 0)
            {
                return $"{passingTime.Minutes} {Messages.DateTimePassingMinutes}";
            }
            return Messages.DateTimeNow;
        }
    }
}
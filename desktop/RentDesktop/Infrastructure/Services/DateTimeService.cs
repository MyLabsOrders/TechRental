using System;
using System.Globalization;

namespace RentDesktop.Infrastructure.Services
{
    internal static class DateTimeService
    {
        public static string DateTimeToString(DateTime dateTime)
        {
            string fullDate = dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);

            int timeIndex = fullDate.IndexOf('T');

            return timeIndex > 0
                ? fullDate.Remove(timeIndex)
                : fullDate;
        }

        public static DateTime StringToDateTime(string data)
        {
            return DateTime.Parse(data);
        }
    }
}

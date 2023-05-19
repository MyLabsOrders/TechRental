using System;
using System.Globalization;

namespace RentDesktop.Infrastructure.Services
{
    internal static class DateTimeService
    {
        public static string DateTimeToString(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);
        }

        public static DateTime StringToDateTime(string data)
        {
            return DateTime.Parse(data);
        }
    }
}

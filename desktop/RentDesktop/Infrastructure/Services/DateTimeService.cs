using System;
using System.Globalization;

namespace RentDesktop.Infrastructure.Services
{
    internal static class DateTimeService
    {
        private const string DATE_FORMAT = "yyyy-MM-dd";
        private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

        public static string DateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString(DATE_FORMAT, FormatProvider);
        }

        public static DateTime StringToDateTime(string data)
        {
            return DateTime.ParseExact(data, DATE_FORMAT, FormatProvider);
        }
    }
}

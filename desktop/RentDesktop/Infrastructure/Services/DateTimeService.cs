using System;
using System.Globalization;

namespace RentDesktop.Infrastructure.Services
{
    internal static class DateTimeService
    {
        private const string SHORT_DATE_FORMAT = "yyyy-MM-dd";
        private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

        public static string ShortDateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString(SHORT_DATE_FORMAT, FormatProvider);
        }

        public static DateTime StringToShortDateTime(string data)
        {
            return DateTime.ParseExact(data, SHORT_DATE_FORMAT, FormatProvider);
        }

        public static string DateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString("o", FormatProvider);
        }

        public static DateTime StringToDateTime(string data)
        {
            return DateTime.Parse(data, FormatProvider);
        }
    }
}

using System;
using System.Globalization;

namespace Grid.Infrastructure.Extensions
{
    public static class DateExtensions
    {
        public static string ToUsFormat(this DateTime value)
        {
            return value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        }

        public static DateTime ToLocalDateTime(this DateTime value)
        {
            try
            {
                var istdate = TimeZoneInfo.ConvertTimeFromUtc(value, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                return istdate;
            }
            catch (Exception ex)
            {
                return value;
            }
        }
    }
}

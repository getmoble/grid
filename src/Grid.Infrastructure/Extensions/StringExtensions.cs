using System;
using System.Globalization;

namespace Grid.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static DateTime DateFromUsFormat(this string value)
        {
            return DateTime.ParseExact(value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }

        public static string Truncate(this string value, int maxLength = 50)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > maxLength)
                    return value.Substring(0, maxLength) + "...";

                return value;
            }

            return "";
        }

        public static string RemoveComma(this string value)
        {
            return !string.IsNullOrEmpty(value) ? value.Replace(',', ';') : value;
        }

        public static string RemoveCommaAndEnterKey(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var returnValue = value.Replace("\r\n", " ");
                var returnValue1 = returnValue.Replace('\n', ' ');
                returnValue1 = returnValue1.Replace(',', ';');
                return returnValue1;
            }
            else
            {
                return value;
            }           
        }
    }
}

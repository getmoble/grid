using System;
using System.Text;

namespace Grid.Providers.Email
{
    public static class IcsHelper
    {
        public static string Generate(DateTime startDate, DateTime endDate, string title, string description)
        {
            string[] contents = { "BEGIN:VCALENDAR",
                                  "BEGIN:VEVENT",
                                  "DTSTART:" + startDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                  "DTEND:" + endDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                  "DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + description,
                                  "SUMMARY:" + title, "PRIORITY:3",
                                  "END:VEVENT",
                                  "END:VCALENDAR" };

            return ConvertStringArrayToString(contents);
        }

        public static string Generate(DateTime date, string title, string description)
        {
            string[] contents = { "BEGIN:VCALENDAR",
                                  "BEGIN:VEVENT",
                                  "DTSTART:" + date.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                  "DTEND:" + date.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                  "DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + description,
                                  "SUMMARY:" + title, "PRIORITY:3",
                                  "END:VEVENT",
                                  "END:VCALENDAR" };

            return ConvertStringArrayToString(contents);
        }

        static string ConvertStringArrayToString(string[] array)
        {
            var builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.AppendLine(value);
            }
            return builder.ToString();
        }
    }
}

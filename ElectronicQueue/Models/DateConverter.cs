using System;

namespace ElectronicQueue.Models
{
    public static class DateConverter
    {
        public static DateTime StringToDateTime(string str) => DateTime.ParseExact(str, "yyyy-MM-dd HH:mm:ss", null);

        public static string DateTimeToString(DateTime dateTime) => dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

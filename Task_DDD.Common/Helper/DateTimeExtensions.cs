using System.Globalization;

namespace Task_DDD.Common.Helper;

public static class DateTimeExtensions
{
    public static string ToShamsiDateString(this DateTimeOffset gregorianDate, string splitter = "/")
    {
        return ToShamsiDateString(gregorianDate.Date, splitter);
    }

    public static string ToShamsiDateString(this DateTime gregorianDate, string splitter = "/")
    {
        var persianCalendar = new PersianCalendar();
        return $"{persianCalendar.GetYear(gregorianDate)}{splitter}{persianCalendar.GetMonth(gregorianDate):D2}{splitter}{persianCalendar.GetDayOfMonth(gregorianDate):D2}";
    }
}
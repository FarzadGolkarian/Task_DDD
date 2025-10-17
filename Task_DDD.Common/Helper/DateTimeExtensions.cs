using System.Globalization;

namespace Task_DDD.Common.Helper;

public static class DateTimeExtensions
{
    public static string ToShamsiDateString(this DateTimeOffset gregorianDate, string splitter = "/")
    {
        return ToShamsiDateString(gregorianDate.Date, splitter);
    }
   
}
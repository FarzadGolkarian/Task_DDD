namespace Task_DDD.Common.Helper;

public static class StringExtension
{
   
    public static string SafeTrim(this string str)
    {
        return (str ?? string.Empty).Trim();
    }


}
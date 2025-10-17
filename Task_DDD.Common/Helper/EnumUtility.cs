using System.ComponentModel;
using System.Reflection;
using Task_DDD.Common.Exceptions;

namespace Task_DDD.Common.Helper
{
    public static class EnumUtility
    {
        public static void ValidationEnumDefined( Type enumType , Enum ename , string value)
        {
            if (!enumType.IsEnumDefined(ename))
            
                throw new BusinessException(string.Format(ErrorMessages.ValidationEnumType, value));
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo field = type.GetField(value.ToString());
            if (field == null)
            {
                return string.Empty;
            }

            DescriptionAttribute[] array =
                (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
            if (array.Length != 0)
            {
                return array[0].Description;
            }

            return Enum.GetName(type, value);
        }
    }
}

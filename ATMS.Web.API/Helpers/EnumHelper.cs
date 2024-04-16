using System;
using System.ComponentModel;
using System.Reflection;

namespace ATMS.Web.API.Helpers
{
    public class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString())!;

            DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))!;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static string GetEnumDescriptionByValue<TEnum>(int value)
        {
            foreach (Enum enumValue in Enum.GetValues(typeof(TEnum)))
            {
                if (Convert.ToInt32(enumValue) == value)
                {
                   return GetDescription(enumValue);
                }
            }

            return null;
        }
    }
}

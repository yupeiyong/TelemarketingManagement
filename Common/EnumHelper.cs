using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;


namespace Common
{

    public static class EnumHelper
    {

        public static string GetEnumDescription<TEnum>(this TEnum source)
        {
            var t = typeof(TEnum);
            if (!t.IsEnum) return string.Empty;
            var propertyName = Enum.GetName(typeof(TEnum), source);

            var field = t.GetField(propertyName);
            if (field == null)
                return string.Empty;

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute == null ? string.Empty : attribute.Description;
        }
    }

}
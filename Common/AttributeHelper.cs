using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class AttributeHelper
    {
        public static string GetDescription(this Type source)
        {
            if (source == null) return string.Empty;
            var attribute = source.GetCustomAttribute<DescriptionAttribute>();
            return attribute == null ? string.Empty : attribute.Description;
        }
    }
}

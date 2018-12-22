﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sparrow.Web.Common
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null) return null;
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T ToEnum<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            var result = Enum.Parse(typeof(T), value, true);
            return (T)result;
        }
    }
}

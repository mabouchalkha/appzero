﻿using System.ComponentModel;
using System.Linq;

namespace System
{
    public static class EnumExtensions
    {
        public static string GetName(this Enum value)
            => Enum.GetName(value.GetType(), value);

        public static string GetDescription(this Enum value)
            => value
                    .GetType()
                    .GetField(value.GetName())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault()
                    .Map(o => o as DescriptionAttribute)
                    ?.Description ?? value.GetName();
    }
}

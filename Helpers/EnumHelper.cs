using System;
using System.Collections.Generic;

namespace Training.Helpers
{
    public static class EnumHelper
    {
        public static string GetDisplayValue(object value)
        {
            if (value == null)
                return "";
            var fieldInfo = value.GetType().GetField(value.ToString());
            return AttributeHelper.GetDisplayName(fieldInfo);
        }
    }
    public static class EnumHelper<T> where T : struct, Enum
    {

        public class EnumItem
        {
            public T Value { get; set; }
            public string DisplayName { get; set; }
        }

        public static IEnumerable<EnumItem> GetEnumItems()
        {
            foreach (var value in Enum.GetValues<T>())
                yield return new EnumItem { Value = value, DisplayName = GetDisplayValue(value) };
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = typeof(T).GetField(value.ToString());
            return AttributeHelper.GetDisplayName(fieldInfo);
        }
    }
}
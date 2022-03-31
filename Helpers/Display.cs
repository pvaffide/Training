using System;
using System.Linq.Expressions;

namespace Training.Helpers
{
    public static class Display
    {
        public static string For(Expression<Func<object>> e, object value)
        {
            if (value == null)
                return "";
            if (value.GetType().IsEnum)
                return EnumHelper.GetDisplayValue(value);
            else if (value is bool @bool)
                return @bool ? "Oui" : "Non";
            return string.Format(AttributeHelper.GetDataFormatString(e), value);
        }
    }
}

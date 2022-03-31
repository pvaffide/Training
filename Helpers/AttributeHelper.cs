using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Training.Helpers
{
    public static class AttributeHelper
    {
        public static string GetDisplayName(Expression<Func<object>> expression)
        {
            return GetDisplayName(GetMemberInfo(expression));
        }


        public static string GetDataFormatString(Expression<Func<object>> expression)
        {
            return GetDataFormatString(GetMemberInfo(expression));
        }
        public static string GetDataFormatString<TValue>(Expression<Func<TValue>> expression)
        {
            return GetDataFormatString(GetMemberInfo(expression));
        }

        public static bool IsRequired(Expression<Func<object>> expression)
        {
            var member = GetMemberInfo(expression);
            return HasRequiredAttribute(member) || IsNonNullableValueType(member);
        }
        public static string GetDisplayName(MemberInfo memberInfo)
        {
            string displayName = ((DisplayAttribute)memberInfo.GetCustomAttribute(typeof(DisplayAttribute), true))?.Name;
            if (displayName == null)
                displayName = ((DisplayNameAttribute)memberInfo.GetCustomAttribute(typeof(DisplayNameAttribute), true))?.DisplayName;

            return displayName ?? memberInfo.Name;
        }
        public static string GetDataFormatString(MemberInfo memberInfo)
        {
            return ((DisplayFormatAttribute)memberInfo.GetCustomAttribute(typeof(DisplayFormatAttribute), true))?.DataFormatString ?? "{0}";
        }

        private static PropertyInfo GetPropertyInfo<TModel>(string expression)
        {
            var properties = expression.Split('.');
            Type type = typeof(TModel);
            PropertyInfo propertyInfo = null;

            foreach (string p in properties)
            {
                propertyInfo = type.GetProperty(p);

                type = propertyInfo.PropertyType;
            }
            return propertyInfo;
        }

        public static string GetDisplayName<TModel>(string expression)
        {
            return GetDisplayName(GetPropertyInfo<TModel>(expression));
        }
        public static string GetDataFormatString<TModel>(string expression)
        {
            return GetDataFormatString(GetPropertyInfo<TModel>(expression));
        }

        private static bool IsNonNullableValueType(MemberInfo memberInfo)
        {
            Type type = memberInfo.GetUnderlyingType();

            if (!type.IsValueType) return false; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return false; // Nullable<T>
            return true; // value-type
        }
        private static bool HasRequiredAttribute(MemberInfo memberInfo)
        {
            return ((RequiredAttribute)memberInfo.GetCustomAttribute(typeof(RequiredAttribute), true)) != null;
        }

        private static MemberInfo GetMemberInfo(Expression<Func<object>> expression)
        {
            return expression.Body.NodeType switch
            {
                ExpressionType.Convert or ExpressionType.ConvertChecked => ((MemberExpression)(expression.Body as UnaryExpression).Operand).Member,
                _ => ((MemberExpression)expression.Body).Member,
            };
        }
        private static MemberInfo GetMemberInfo<TValue>(Expression<Func<TValue>> expression)
        {
            return expression.Body.NodeType switch
            {
                ExpressionType.Convert or ExpressionType.ConvertChecked => ((MemberExpression)(expression.Body as UnaryExpression).Operand).Member,
                _ => ((MemberExpression)expression.Body).Member,
            };
        }

        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                     "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }
    }
}

using System.Reflection;


namespace TicketsGeneratorServices.Common.Extensions
{
    public static class TypeExtensions
    {
        public static bool? CheckIsNullable(PropertyInfo property) =>
            property.PropertyType.CheckIsNullable(property);

        public static bool? CheckIsNullable(FieldInfo field) =>
            field.FieldType.CheckIsNullable(field);

        public static bool? CheckIsNullable(ParameterInfo parameter) =>
            parameter.ParameterType.CheckIsNullable(parameter.Member);

        private static bool? CheckIsNullable(this Type type, MemberInfo memberInfo)
        {
            if (type.IsValueType)
                return Nullable.GetUnderlyingType(type) != null;

            var nullableAttribute = memberInfo.GetCustomAttribute<System.Runtime.CompilerServices.NullableAttribute>();
            if (nullableAttribute != null && nullableAttribute.NullableFlags.Length >= 1)
                return nullableAttribute.NullableFlags[0] == 2;

            for (var declaringType = memberInfo.DeclaringType; type != null; declaringType = declaringType?.DeclaringType)
            {
                var nullableContextAttribute = declaringType!.GetCustomAttribute<System.Runtime.CompilerServices.NullableContextAttribute>();
                if (nullableContextAttribute != null)
                    return nullableContextAttribute.Flag == 2;
            }

            // Couldn't find a suitable attribute
            return null;
        }



        public static bool? CheckEnumerableElementIsNullable(this PropertyInfo enumerableInfo)
        {
            var enumerableType = enumerableInfo.PropertyType;
            if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(enumerableType))
                return null; // Recieved type is not Enumerable

            var elementType = enumerableType.HasElementType
                ? enumerableType.GetElementType()
                : enumerableType.GetGenericArguments().FirstOrDefault();

            if (elementType == null)
                return null; // Could not find element

            // ValueType
            if (elementType.IsValueType)
                return Nullable.GetUnderlyingType(elementType) != null;

            // ReferenceType
            var nullableAttribute = enumerableInfo.GetCustomAttribute<System.Runtime.CompilerServices.NullableAttribute>();
            if (nullableAttribute != null && nullableAttribute.NullableFlags.Length >= 2)
                return nullableAttribute.NullableFlags[1] == 2; // Take second flag from nullable attribute for first generic type

            // Couldn't find a suitable attribute
            return null;
        }
    }
}
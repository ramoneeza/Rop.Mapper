using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Mapper
{
    public class TypeProxy
    {
        public Type Type { get; }
        public bool IsNullAllowed { get; }
        public string Name => Type.Name;
        public Type? BaseType { get; }
        public bool IsArray { get; }
        public bool IsNullable { get; }
        public bool IsList { get; }
        public bool IsEnumerable { get; }
        public bool IsEnum { get; }
        public bool IsString { get; }
        public string Separator { get; }
        public string Format { get; }
        public object? DefaultValue { get; }
        public TypeCode TypeCode { get; }

        public TypeProxy(Type type, bool isNullAllowed, string? format = null, string? separator = null,
            object? defaultvalue = null)
        {
            Type = type;
            TypeCode = Type.GetTypeCode(type);
            IsNullAllowed = isNullAllowed;
            Format = format ?? "";
            Separator = separator ?? ",";
            var nulltype = Nullable.GetUnderlyingType(type);
            if (nulltype != null)
            {
                IsNullable = true;
                IsNullAllowed = true;
                BaseType = nulltype;
                DefaultValue = null;
                return;
            }

            if (TypeCode == TypeCode.String)
            {
                IsString = true;
                DefaultValue = (IsNullAllowed) ? null : defaultvalue?.ToString() ?? "";
                return;
            }

            if (type.IsArray)
            {
                IsArray = true;
                IsEnumerable = true;
                BaseType = type.GetElementType()!;
                if (!isNullAllowed) DefaultValue = Array.CreateInstance(BaseType, 0);
                return;
            }

            if (type.IsEnum)
            {
                IsEnum = true;
                BaseType = type.GetEnumUnderlyingType();
                if (!isNullAllowed) DefaultValue = defaultvalue ?? 0;
                return;
            }

            if (type.IsIEnumerableOfT(out var enumtype))
            {
                BaseType = enumtype;
                IsEnumerable = true;
                if (type.IsIListOfT(out enumtype))
                {
                    IsList = true;
                    enumtype = nulltype;
                }

                if (!isNullAllowed) DefaultValue = Activator.CreateInstance(type);
                return;
            }

            if (!isNullAllowed)
            {
                DefaultValue = defaultvalue ?? Activator.CreateInstance(type);
            }
            else
            {
                if (defaultvalue is not null) DefaultValue = defaultvalue;
            }
        }

        public override string ToString()
        {
            return $"{Type.Name}({BaseType?.Name}){(IsNullAllowed?"(?)":"")}";
        }
    }
}

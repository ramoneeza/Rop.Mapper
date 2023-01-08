using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Rules;
using Rop.Types;

namespace Rop.Mapper
{
    public static class ConversionEngine
    {
        public static object? ConvertValue(object? valuesrc, PropertyType typesrc,PropertyType typedst, IConverter? desireconverter)
        {
            // Null VALUE
            if (valuesrc == null)
            {
                if (typedst.TypeProxy.IsNullAllowed || typedst.TypeProxy.IsNullable) return null;
                if (desireconverter is not null && desireconverter.CanConvertNull) return desireconverter.Convert(valuesrc, typesrc, typedst);
                var dv = typedst.GetDefaultValue();
                return dv ?? throw new Exception("Null not allowed for Conversion");
            }
            //
            // Nullable VALUE
            if (typedst.TypeProxy.IsNullable)
            {
                var newdst = typedst with {TypeProxy = typedst.TypeProxy.BaseType!};
                return ConvertValue(valuesrc, typesrc,newdst, desireconverter);
            }
            //
            // Desired Converter
            if (desireconverter is not null) return desireconverter.Convert(valuesrc, typesrc, typedst);
            //
            // Same Type
            if (typesrc.Type == typedst.Type)
            {
                return valuesrc;
            }
            //
            // StringConverter
            if (typedst.TypeProxy.IsString)
            {
                return ConvertValueToString(valuesrc, typesrc,typedst);
            }
            //
            // EnumConverter
            if (typedst.TypeProxy.IsEnum)
            {
                return ConvertValueToEnum(valuesrc, typesrc, typedst);
            }
            //
            // IEnumerable To IEnumerable Converter
            if (typesrc.TypeProxy.IsEnumerable && typedst.TypeProxy.IsEnumerable)
            {
                return ConvertEnumerableToEnumerable(valuesrc, typesrc, typedst);
            }
            //
            // String To IEnumerable Converter
            if (typesrc.TypeProxy.IsString && typedst.TypeProxy.IsEnumerable)
            {
                var s = (valuesrc as string)!;
                return ConvertStringToEnumerable(s, typesrc,typedst);
            }
            //
            // Raw Conversion
            var resfinal = Convert.ChangeType(valuesrc, typedst.Type);
            return resfinal;
        }

        private static string ConvertSimpleValueToString(object value, string? format)
        {
            // No DesiredFormat or not IFormattable value
            if (string.IsNullOrEmpty(format) || value is not IFormattable fvalue)
                return value.ToString() ?? "";
            else
                return fvalue.ToString(format, CultureInfo.InvariantCulture);
        }


        private static string ConvertValueToString(object valuesrc, PropertyType origin,PropertyType destiny)
        {
            var format =destiny.DecoFormat;
            // Primitive value
            if (origin.TypeProxy.TypeCode != TypeCode.Object)
            {
                ConvertSimpleValueToString(valuesrc, format);
            }
            // Array or List
            if (origin.TypeProxy.IsArray || origin.TypeProxy.IsList)
            {
                var separator = destiny.DecoSeparator;
                if (valuesrc is not IEnumerable valuesrcenumerable)
                    throw new InvalidCastException("Can't convert to IEnumerable");
                var list = valuesrcenumerable.Cast<object>().Select(o => ConvertSimpleValueToString(o, format))
                    .ToList();
                return string.Join(separator, list);
            }
            // Default conversion
            return ConvertSimpleValueToString(valuesrc, format);
        }

        private static object ConvertValueToEnum(object valuesrc, PropertyType origin,PropertyType destiny)
        {
            // String to Enum
            if (origin.TypeProxy.IsString)
                return Enum.Parse(destiny.Type, valuesrc?.ToString() ?? "", true);
            //
            // Other types
            var enumt = destiny.TypeProxy.BaseType!;
            // Same base type
            if (origin.TypeProxy.TypeCode == Type.GetTypeCode(enumt.Type)) return valuesrc;
            // Different base tipo
            var e = Convert.ChangeType(valuesrc, enumt.Type);
            return e;
        }
        private static object ConvertEnumerableToEnumerable(object valuesrc,PropertyType origin,PropertyType destiny)
        {
            if (origin.TypeProxy.BaseType != destiny.TypeProxy.BaseType) throw new InvalidCastException($"Can't change IEnumerable of {origin.TypeProxy.BaseType} to IEnumerable of {destiny.TypeProxy.BaseType}");
            var ve = valuesrc as IEnumerable;
            if (destiny.TypeProxy.IsArray)
            {
                return EnumerableHelper.CastToArray(ve!, destiny.TypeProxy.BaseType!.Type);
            }
            if (destiny.TypeProxy.IsList)
            {
                return EnumerableHelper.CastToList(ve!, destiny.TypeProxy.BaseType!.Type);
            }
            throw new InvalidCastException($"Can't change {origin} to {destiny}");
        }

        private static object ConvertStringToEnumerable(string valuesrc,PropertyType origin,PropertyType destiny)
        {
            var separator = origin.DecoSeparator?.FirstOrDefault()??',';
            var format = destiny.DecoFormat;
            var ve = valuesrc.Split(separator);
            if (destiny.TypeProxy.BaseType!.Type != typeof(string))
            {
                var vei = ve.Select(s => Convert.ChangeType(s, destiny.TypeProxy.BaseType!.Type)).ToList();
                var neworigin =origin with {TypeProxy = TypeProxy.Get(vei.GetType(), false)};
                return ConvertEnumerableToEnumerable(vei, neworigin, destiny);
            }
            else
            {
                var neworigin = origin with { TypeProxy = TypeProxy.Get(ve.GetType(), false) };
                return ConvertEnumerableToEnumerable(ve, neworigin, destiny);
            }
        }
    }
    
}

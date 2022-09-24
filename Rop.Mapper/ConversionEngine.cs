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
        public static object? ConvertValue(object? valuesrc, ITypeProxy typesrc,TypeDecorator srcDecorator, ITypeProxy typedst,TypeDecorator dstDecorator, IConverter? desireconverter)
        {
            // Null VALUE
            if (valuesrc == null)
            {
                if (typedst.IsNullAllowed || typedst.IsNullable) return null;
                if (desireconverter is not null && desireconverter.CanConvertNull) return desireconverter.Convert(valuesrc, typesrc, typedst);
                return typedst.GetDefaultValue() ?? typedst.Type.GetDefaultValue() ?? throw new Exception("Null not allowed for Conversion");
            }
            //
            // Nullable VALUE
            if (typedst.IsNullable)
            {
                var newdst = typedst.BaseType!;
                return ConvertValue(valuesrc, typesrc,srcDecorator, newdst,dstDecorator, desireconverter);
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
            if (typedst.IsString)
            {
                return ConvertValueToString(valuesrc, typesrc,srcDecorator, typedst,dstDecorator);
            }
            //
            // EnumConverter
            if (typedst.IsEnum)
            {
                return ConvertValueToEnum(valuesrc, typesrc, typedst);
            }
            //
            // IEnumerable To IEnumerable Converter
            if (typesrc.IsEnumerable && typedst.IsEnumerable)
            {
                return ConvertEnumerableToEnumerable(valuesrc, typesrc, typedst);
            }
            //
            // String To IEnumerable Converter
            if (typesrc.IsString && typedst.IsEnumerable)
            {
                var s = (valuesrc as string)!;
                return ConvertStringToEnumerable(s, typesrc,srcDecorator, typedst,dstDecorator);
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


        private static string ConvertValueToString(object valuesrc, ITypeProxy origin,TypeDecorator srcDecorator, ITypeProxy destiny,TypeDecorator destinyDecorator)
        {
            var format = destinyDecorator.Format;
            // Primitive value
            if (origin.TypeCode != TypeCode.Object)
            {
                ConvertSimpleValueToString(valuesrc, format);
            }
            // Array or List
            if (origin.IsArray || origin.IsList)
            {
                var separator = destinyDecorator.Separator;
                if (valuesrc is not IEnumerable valuesrcenumerable)
                    throw new InvalidCastException("Can't convert to IEnumerable");
                var list = valuesrcenumerable.Cast<object>().Select(o => ConvertSimpleValueToString(o, format))
                    .ToList();
                return string.Join(separator, list);
            }
            // Default conversion
            return ConvertSimpleValueToString(valuesrc, format);
        }

        private static object ConvertValueToEnum(object valuesrc, ITypeProxy origin, ITypeProxy destiny)
        {

            // String to Enum
            if (origin.IsString)
                return Enum.Parse(destiny.Type, valuesrc?.ToString() ?? "", true);
            //
            // Other types
            var enumt = destiny.BaseType!;
            // Same base type
            if (origin.TypeCode == Type.GetTypeCode(enumt.Type)) return valuesrc;
            // Different base tipo
            var e = Convert.ChangeType(valuesrc, enumt.Type);
            return e;
        }

        private static object ConvertEnumerableToEnumerable(object valuesrc, ITypeProxy origin, ITypeProxy destiny)
        {
            if (origin.BaseType != destiny.BaseType) throw new InvalidCastException($"Can't change IEnumerable of {origin.BaseType} to IEnumerable of {destiny.BaseType}");
            var ve = valuesrc as IEnumerable;
            if (destiny.IsArray)
            {
                return EnumerableHelper.CastToArray(ve!, destiny.BaseType!.Type);
            }
            if (destiny.IsList)
            {
                return EnumerableHelper.CastToList(ve!, destiny.BaseType!.Type);
            }
            throw new InvalidCastException($"Can't change {origin} to {destiny}");
        }

        private static object ConvertStringToEnumerable(string valuesrc, ITypeProxy origin,TypeDecorator origindecorator,  ITypeProxy destiny,TypeDecorator destinyDecorator)
        {
            var separator = origindecorator.Separator?.FirstOrDefault()??',';
            var format = destinyDecorator.Format;
            var ve = valuesrc.Split(separator);
            if (destiny.BaseType!.Type != typeof(string))
            {
                var vei = ve.Select(s => Convert.ChangeType(s, destiny.BaseType!.Type)).ToList();
                var neworigin =TypeProxy.Get(vei.GetType(), false);
                return ConvertEnumerableToEnumerable(vei, neworigin, destiny);
            }
            else
            {
                var neworigin = TypeProxy.Get(ve.GetType(), false);
                return ConvertEnumerableToEnumerable(ve, neworigin, destiny);
            }
        }
    }
    
}

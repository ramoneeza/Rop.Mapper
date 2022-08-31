using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;

namespace Rop.Converters
{
    



    public class ConversionEngine
    {
#region explicit converters
        private readonly ConcurrentDictionary<string, IConverter> _converterdicbyname = new ConcurrentDictionary<string, IConverter>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, IConverter> _converterdicbytype = new ConcurrentDictionary<string, IConverter>(StringComparer.OrdinalIgnoreCase);

        public void RegisterConverterByName(IConverter converter, bool force = false, bool includetypes = false)
        {
            if (_converterdicbyname.ContainsKey(converter.Name) && !force) return;
            _converterdicbyname[converter.Name] = converter;
            if (includetypes)
                RegisterConverterByTypes(converter, force, false);
        }
        public void RegisterConverterByTypes(IConverter converter, bool force = false, bool includename = false)
        {
            if (_converterdicbytype.ContainsKey(converter.Key) && !force) return;
            switch (converter)
            {
                case IConverterSymmetric cs:
                    var ab = cs.ConverterBA;
                    var ba = cs.ConverterBA;
                    _converterdicbytype[ab.Key] = ab;
                    _converterdicbytype[ba.Key] = ba;
                    break;
                default:
                    _converterdicbytype[converter.Key] = converter;
                    break;
            }
            if (includename) RegisterConverterByName(converter, force, false);
        }
        #endregion
        private static object? GetDefaultValue(Type t)
        {
            if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                return Activator.CreateInstance(t);
            else
                return null;
        }

        public A DirectConvertValue<A>(A valuesrc, IConverter<A,A> desiredconverter = null)
        {
            if (desiredconverter is not null) return desiredconverter.Convert(valuesrc);
            return valuesrc;
        }
        public A DirectConvertValue<A>(A? valuesrc, IConverter<A?, A> desiredconverter = null) where A:struct
        {
            if (desiredconverter is not null) return desiredconverter.Convert(valuesrc);
            return valuesrc??default;
        }
        public A? DirectConvertValue<A>(A? valuesrc, IConverter<A?, A?> desiredconverter = null) where A : struct
        {
            if (desiredconverter is not null) return desiredconverter.Convert(valuesrc);
            return valuesrc;
        }
        
        public B DirectConvertValue<A, B>(A valuesrc, IConverter desiredconverter=null)
        {
            if (desiredconverter is IConverter<A, B> icab) return icab.Convert(valuesrc);
            if (valuesrc is null) return default;
            if (typeof(A) == typeof(B)) return (B)(object)valuesrc;

            var basenullable = Nullable.GetUnderlyingType(typeof(B));
            if (basenullable != null) return (B)DirectConvertValue(valuesrc,typeof(A), basenullable, desiredconverter);
        }


        public object DirectConvertValue(object valuesrc, Type typesrc, Type typedst,IConverter desiredconverter)
        {
            // Null VALUE
            if (valuesrc == null)
            {
                if (bNullable) 
                    return null;
                else
                    return GetDefaultValue(typedst)?? throw new Exception("Null not allowed for Conversion");
            }
            //
            // Nullable VALUE
            var basenullable = Nullable.GetUnderlyingType(typedst);
            if (basenullable != null)
            {
                return DirectConvertValue(valuesrc, typesrc,aNullable, basenullable,false, desiredconverter);
            }
            //
            // Desired Converter
            if (desiredconverter is not null) return desiredconverter.Convert(valuesrc);
            //
            // Same Type
            if (typesrc == typedst)
            {
                return valuesrc;
            }
            //
            // StringConverter
            if (Type.GetTypeCode(typedst)==TypeCode.String)
            {
                return ConvertValueToString(valuesrc, typesrc, typedst);
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
                return ConvertStringToEnumerable(s, typesrc, typedst);
            }
            //
            // Raw Conversion
            var resfinal = Convert.ChangeType(valuesrc, typedst.Type);
            return resfinal;
        }

        private static string ConvertSimpleValueToString(object value, string format)
        {
            // No DesiredFormat or not IFormattable value
            if (string.IsNullOrEmpty(format) || value is not IFormattable fvalue)
                return value.ToString() ?? "";
            else
                return fvalue.ToString(format, CultureInfo.InvariantCulture);
        }


        private static string ConvertValueToString(object valuesrc, TypeProxy origin, TypeProxy destiny)
        {
            var format = destiny.Format;
            // Primitive value
            if (origin.TypeCode != TypeCode.Object)
            {
                ConvertSimpleValueToString(valuesrc, format);
            }
            // Array or List
            if (origin.IsArray || origin.IsList)
            {
                var separator = destiny.Separator;
                if (valuesrc is not IEnumerable valuesrcenumerable)
                    throw new InvalidCastException("Can't convert to IEnumerable");
                var list = valuesrcenumerable.Cast<object>().Select(o => ConvertSimpleValueToString(o, format))
                    .ToList();
                return string.Join(separator, list);
            }
            // Default conversion
            return ConvertSimpleValueToString(valuesrc, format);
        }

        private static object ConvertValueToEnum(object valuesrc, TypeProxy origin, TypeProxy destiny)
        {

            // String to Enum
            if (origin.IsString)
                return Enum.Parse(destiny.Type, valuesrc?.ToString() ?? "", true);
            //
            // Other types
            var enumt = destiny.BaseType!;
            // Same base type
            if (origin.TypeCode == Type.GetTypeCode(enumt)) return valuesrc;
            // Different base tipo
            var e = Convert.ChangeType(valuesrc, enumt);
            return e;
        }

        private static object ConvertEnumerableToEnumerable(object valuesrc, TypeProxy origin, TypeProxy destiny)
        {
            if (origin.BaseType != destiny.BaseType) throw new InvalidCastException($"Can't change IEnumerable of {origin.BaseType} to IEnumerable of {destiny.BaseType}");
            var ve = valuesrc as IEnumerable;
            if (destiny.IsArray)
            {
                return TypeHelper.CreateArray(ve!, destiny.BaseType!);
            }
            if (destiny.IsList)
            {
                return TypeHelper.CreateList(ve!, destiny.BaseType!);
            }
            throw new InvalidCastException($"Can't change {origin} to {destiny}");
        }

        private static object ConvertStringToEnumerable(string valuesrc, TypeProxy origin, TypeProxy destiny)
        {
            var separator = origin.Separator.FirstOrDefault();
            var format = destiny.Format;
            var ve = valuesrc.Split(separator);
            if (destiny.BaseType != typeof(string))
            {
                var vei = ve.Select(s => Convert.ChangeType(s, destiny.BaseType!)).ToList();
                var neworigin = new TypeProxy(vei.GetType(), false);
                return ConvertEnumerableToEnumerable(vei, neworigin, destiny);
            }
            else
            {
                var neworigin = new TypeProxy(ve.GetType(), false);
                return ConvertEnumerableToEnumerable(ve, neworigin, destiny);
            }
        }

    }
    
}

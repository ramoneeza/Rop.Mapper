using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Rop.Mapper.Rules;
using Rop.Types;

namespace Rop.Mapper.Converters
{
    public abstract class AbsConverter<A,B>:IConverter<A,B>
    {
        public Type AType { get; }
        public Type BType { get; }
        public virtual bool CanConvertNull => false;
        public virtual string Name { get; }

        public virtual bool CanConvertA(Type t) => t == AType;
        public virtual bool CanConvertB(Type t) => t == BType;

        object? IConverter.Convert(object? value,PropertyType typesrc,PropertyType typedst)
        {
            if (!CanConvertA(typesrc.Type) || !CanConvertB(typedst.Type))
            {
                throw new Exception("Bad Conversor");
            }
            return Convert((A) value!);
        }
        public abstract B? Convert(A? value);

        protected AbsConverter(Type? typeSrc=null, Type? typeDst=null,string? name=null)
        {
            this.AType = typeSrc??typeof(A);
            this.BType = typeDst??typeof(B);
            Name = name??this.GetType().Name;
        }

        protected static bool IsTorNT(Type t, Type other)
        {
            return (t == other)  || (Nullable.GetUnderlyingType(t) == other);
        }

        protected static bool IsTorNT<T>(Type t) => IsTorNT(t, typeof(T));
        
    }
    public abstract class AbsSimetricConverter<A,B> :AbsConverter<A,B>,IConverterSymmetric<A,B>
    {
        object? IConverter.Convert(object? value, PropertyType typesrc, PropertyType typedst)
        {
            if (CanConvertA(typesrc.Type) && CanConvertB(typedst.Type)) return Convert((A)value!);
            if (CanConvertB(typesrc.Type) && CanConvertA(typedst.Type)) return Convert((B)value!);
            throw new Exception("Bad Conversor");
        }
        public abstract A? Convert(B? value);
        protected AbsSimetricConverter(Type? typeSrc=null, Type? typeDst=null, string? name=null) : base(typeSrc, typeDst, name)
        {
        }
    }


    public class DateOnlyConverter:AbsSimetricConverter<DateTime,DateOnly>
    {
        public override DateTime Convert(DateOnly value)
        {
            return value.ToDateTime(new TimeOnly(0));
        }
        public override DateOnly Convert(DateTime value)
        {
            return DateOnly.FromDateTime(value);
        }
    }
    public class DateOnlyConverter2 : AbsSimetricConverter<DateTime?, DateOnly?>
    {
        public override bool CanConvertA(Type t) => IsTorNT<DateTime>(t);
        public override bool CanConvertB(Type t) => IsTorNT<DateOnly>(t);
       

        public override DateTime? Convert(DateOnly? value)
        {
            return value?.ToDateTime(new TimeOnly(0));
        }
        public override DateOnly? Convert(DateTime? value)
        {
            if (value is null) return null;
            return DateOnly.FromDateTime(value.Value);
        }
    }

    public class TimeOnlyConverter : AbsSimetricConverter<TimeSpan, TimeOnly>
    {
        public override TimeSpan Convert(TimeOnly value)
        {
            return value.ToTimeSpan();
        }
        public override TimeOnly Convert(TimeSpan value)
        {
            return TimeOnly.FromTimeSpan(value);
        }
    }
    public class TimeOnlyConverter2 : AbsSimetricConverter<TimeSpan?, TimeOnly?>
    {
        public override bool CanConvertA(Type t) => IsTorNT<TimeSpan>(t);
        public override bool CanConvertB(Type t) => IsTorNT<TimeOnly>(t);



        public override TimeSpan? Convert(TimeOnly? value)
        {
            return value?.ToTimeSpan();
        }
        public override TimeOnly? Convert(TimeSpan? value)
        {
            if (value is null) return null;
            return TimeOnly.FromTimeSpan(value.Value);
        }
    }

    public class DateTimeToTimeConverter : AbsConverter<DateTime, TimeOnly>
    {
        public override TimeOnly Convert(DateTime value)
        {
            return TimeOnly.FromDateTime(value);
        }
    }
    public class DateTimeToTimeConverter2 : AbsConverter<DateTime?, TimeOnly?>
    {
        public override bool CanConvertA(Type t) => IsTorNT<DateTime>(t);
        public override bool CanConvertB(Type t) => IsTorNT<TimeOnly>(t);

        public override TimeOnly? Convert(DateTime? value)
        {
            if (value is null) return null;
            return TimeOnly.FromDateTime(value.Value);
        }
    }


    public class BitArrayConverter:AbsSimetricConverter<byte[],BitArray>
    {
        public static BitArray ToBitArray(byte[]? value)=>new BitArray(value??new byte[]{});
        public static byte[] ToBytes(BitArray? value)
        {
            if (value is null || value.Length == 0) return new byte[] { };
            var bytes = new byte[(value.Length - 1) / 8 + 1];
            value.CopyTo(bytes, 0);
            return bytes;
        }

        public static bool Equal(BitArray a, BitArray b)
        {
            if (a.Length!=b.Length) return false;
            if (a.Length == 0) return true;
            var aa = ToBytes(a);
            var bb = ToBytes(b);
            return aa.SequenceEqual(bb);
        }
        public static bool Equal(byte[] a,byte[] b)
        {
            if (a.Length!=b.Length) return false;
            if (a.Length == 0) return true;
            return a.SequenceEqual(b);
        }

        public override BitArray? Convert(byte[]? value) => ToBitArray(value);
        public override byte[]? Convert(BitArray? value) => ToBytes(value);
    }

}

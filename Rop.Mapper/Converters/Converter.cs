using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Rop.Mapper.Rules;

namespace Rop.Mapper.Converters
{
    public abstract class AbsConverter<Src,Dst>:IConverter<Src,Dst>,IConverter
    {
        public abstract string Name { get; }
        object? IConverter.Convert(object? value, TypeProxy typesrc, TypeProxy typedst)
        {
            if (typesrc.Type != typeof(Src) || typedst.Type != typeof(Dst)) throw new Exception("Bad Conversor");
            return Convert((Src) value!);
        }
        public abstract Dst? Convert(Src? value);
        
    }
    public abstract class AbsSimetricConverter<A,B> : IConverterSym<A,B>,IConverter
    {
        public abstract string Name { get; }
        object? IConverter.Convert(object? value, TypeProxy typesrc, TypeProxy typedst)
        {
            if (typesrc.Type==typeof(A)&&typedst.Type==typeof(B)) return Convert((A)value!);
            if (typesrc.Type == typeof(B) && typedst.Type == typeof(A)) return Convert((B)value!);
            throw new Exception("Bad Conversor");
        }
        public abstract A? Convert(B? value);
        public abstract B? Convert(A? value);
    }


    public class DateOnlyConverter:AbsSimetricConverter<DateTime,DateOnly>
    {
        public override string Name => "DateConversor";
        public override DateTime Convert(DateOnly value)
        {
            return value.ToDateTime(new TimeOnly(0));
        }
        public override DateOnly Convert(DateTime value)
        {
            return DateOnly.FromDateTime(value);
        }
    }
    public class TimeOnlyConverter : AbsSimetricConverter<TimeSpan, TimeOnly>
    {
        public override string Name => "TimeConversor";
        public override TimeSpan Convert(TimeOnly value)
        {
            return value.ToTimeSpan();
        }
        public override TimeOnly Convert(TimeSpan value)
        {
            return TimeOnly.FromTimeSpan(value);
        }
    }

    public class DateTimeToTimeConverter : AbsConverter<DateTime, TimeOnly>
    {
        public override string Name => "DateTimeToTime";
        public override TimeOnly Convert(DateTime value)
        {
            return TimeOnly.FromDateTime(value);
        }
    }
}

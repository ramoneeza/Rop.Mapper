using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Rop.Mapper.Rules;

namespace Rop.Mapper.Converters
{
    public abstract class AbsConverter<A,B>:IConverter<A,B>
    {
        public Type AType { get; }
        public Type BType { get; }
        public virtual string Name { get; }
        object? IConverter.Convert(object? value, TypeProxy typesrc, TypeProxy typedst)
        {
            if (typesrc.Type != AType || typedst.Type != BType) throw new Exception("Bad Conversor");
            return Convert((A) value!);
        }
        public abstract B? Convert(A? value);

        protected AbsConverter(Type? typeSrc=null, Type? typeDst=null,string? name=null)
        {
            this.AType = typeSrc??typeof(A);
            this.BType = typeDst??typeof(B);
            Name = name??this.GetType().Name;
        }
    }
    public abstract class AbsSimetricConverter<A,B> :AbsConverter<A,B>,IConverterSymmetric<A,B>
    {
        object? IConverter.Convert(object? value, TypeProxy typesrc, TypeProxy typedst)
        {
            if (typesrc.Type==typeof(A)&&typedst.Type==typeof(B)) return Convert((A)value!);
            if (typesrc.Type == typeof(B) && typedst.Type == typeof(A)) return Convert((B)value!);
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

    public class DateTimeToTimeConverter : AbsConverter<DateTime, TimeOnly>
    {
        public override TimeOnly Convert(DateTime value)
        {
            return TimeOnly.FromDateTime(value);
        }
    }
}

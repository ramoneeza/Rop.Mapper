namespace Rop.Converters.DateTimeConverters;

public class TimeOnlyConverter : SimetricConverter<TimeSpan, TimeOnly>
{
    public TimeSpan RawConvert(TimeOnly value)
    {
        return value.ToTimeSpan();
    }
    public TimeOnly RawConvert(TimeSpan value)
    {
        return TimeOnly.FromTimeSpan(value);
    }
    public override Func<TimeSpan, TimeOnly> Convert => RawConvert;
    public override Func<TimeOnly, TimeSpan> ConvertInv => RawConvert;
}

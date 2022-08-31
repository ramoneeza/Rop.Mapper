namespace Rop.Converters.DateTimeConverters;

public class DateTimeToTimeConverter : Converter<DateTime, TimeOnly>
{
    public TimeOnly RawConvert(DateTime value)
    {
        return TimeOnly.FromDateTime(value);
    }
    public override Func<DateTime, TimeOnly> Convert => RawConvert;
}
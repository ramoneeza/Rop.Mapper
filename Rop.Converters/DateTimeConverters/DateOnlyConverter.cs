namespace Rop.Converters.DateTimeConverters;

public class DateOnlyConverter :SimetricConverter<DateTime, DateOnly>
{
    public DateTime RawConvert(DateOnly value)
    {
        return value.ToDateTime(new TimeOnly(0));
    }
    public DateOnly RawConvert(DateTime value)
    {
        return DateOnly.FromDateTime(value);
    }

    public override Func<DateTime, DateOnly> Convert => RawConvert;
    public override Func<DateOnly, DateTime> ConvertInv => RawConvert;
}

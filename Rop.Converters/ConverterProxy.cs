namespace Rop.Converters;

public class ConverterProxy<A, B> : Converter<A, B>
{
    public override Func<A, B> Convert { get; }

    public ConverterProxy(IConverter converter,string name = null):base(name)
    {
        switch (converter)
        {
            case IConverter<A, B> converter2:
                Convert = converter2.Convert;
                break;
            case IConverterSymmetric<B, A> converters2:
                Convert = converters2.ConvertInv;
                break;
            default:
                throw new ArgumentException("Converter is not A or B");
        }
    }
}
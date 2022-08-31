namespace Rop.Converters;

public abstract class SimetricConverter<A,B> :Converter<A,B>,IConverterSymmetric<A,B>
{
    public string InvKey { get; }

    public IConverter<A,B> ConverterAB { get; }
    public IConverter<B,A> ConverterBA { get; }

    public abstract Func<B, A> ConvertInv { get; }

    IConverter IConverterSymmetric.ConverterAB => ConverterAB;

    IConverter IConverterSymmetric.ConverterBA => ConverterBA;

    protected SimetricConverter(string? name = null) : base(name)
    {
        ConverterAB = new ConverterProxy<A,B>(this,name);
        ConverterBA = new ConverterProxy<B,A>(this,name);
    }
}
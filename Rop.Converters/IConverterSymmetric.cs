namespace Rop.Converters;

public interface IConverterSymmetric : IConverter
{
    string InvKey { get; }

    IConverter ConverterAB { get; }
    IConverter ConverterBA { get; }

    public string InvTypeKey()
    {
        var key =TypeKey(BType,AType);
        return key;
    }
}
public interface IConverterSymmetric<A, B> : IConverter<A, B>, IConverterSymmetric
{
    Func<B,A> ConvertInv { get; }
    IConverter<A,B> ConverterAB { get; }
    IConverter<B,A> ConverterBA { get; }
}
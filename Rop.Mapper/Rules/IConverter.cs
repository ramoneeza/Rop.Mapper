using Rop.Types;

namespace Rop.Mapper.Rules;

public interface IConverter
{
    Type AType { get; }
    Type BType { get; }
    string Name { get; }
    bool CanConvertNull { get; }
    object? Convert(object? value,PropertyType typesrc,PropertyType typedst);
    public string TypeKey()
    {
        var key = TypeKey(AType,BType);
        return key;
    }
    public static string TypeKey(Type a,Type b)
    {
        var ta =TypeProxy.Get(a);
        var tb=TypeProxy.Get(b);
        return $"{ta.FriendlyName}|{tb.FriendlyName}";
    }
}

public interface IConverter<A, B>:IConverter
{
    B? Convert(A? value);
}

public interface IConverterSymmetric : IConverter
{
    public string InvTypeKey()
    {
        var ta = TypeProxy.Get(AType);
        var tb = TypeProxy.Get(BType);
        var key = $"{tb.FriendlyName}|{ta.FriendlyName}";
        return key;
    }
}
public interface IConverterSymmetric<A,B>:IConverter<A,B>,IConverterSymmetric
{
    A? Convert(B? value);
}
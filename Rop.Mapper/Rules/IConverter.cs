namespace Rop.Mapper.Rules;

public interface IConverter
{
    Type AType { get; }
    Type BType { get; }
    string Name { get; }
    bool CanConvertNull { get; }
    object? Convert(object? value,TypeProxy typesrc,TypeProxy typedst);
    public string TypeKey()
    {
        var key = TypeKey(AType,BType);
        return key;
    }
    public static string TypeKey(Type a,Type b)=>$"{a.Name}|{b.Name}";
}

public interface IConverter<A, B>:IConverter
{
    B? Convert(A? value);
}

public interface IConverterSymmetric : IConverter
{
    public string InvTypeKey()
    {
        var key = $"{BType.Name}|{AType.Name}";
        return key;
    }
}
public interface IConverterSymmetric<A,B>:IConverter<A,B>,IConverterSymmetric
{
    A? Convert(B? value);
}
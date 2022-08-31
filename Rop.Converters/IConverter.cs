namespace Rop.Converters;

public interface IConverter
{
    Type AType { get; }
    Type BType { get; }
    string Name { get; }
    string Key { get; }
    public string TypeKey()
    {
        var key = TypeKey(AType,BType);
        return key;
    }
    public static string TypeKey(Type a,Type b)=>$"{a.Name}|{b.Name}";
}

public interface IConverter<A, B>:IConverter
{
    Func<A,B> Convert { get; }
}



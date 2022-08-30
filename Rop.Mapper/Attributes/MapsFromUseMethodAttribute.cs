namespace Rop.Mapper.Attributes;

public class MapsFromUseMethodAttribute : Attribute,IMapsFromAttribute
{
    public Type Src { get; }
    public string Method { get; }
    public MapsFromUseMethodAttribute(Type src,string method)
    {
        Src = src;
        Method = method;
    }
}
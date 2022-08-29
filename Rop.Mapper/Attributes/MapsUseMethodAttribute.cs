namespace Rop.Mapper.Attributes;

public class MapsUseMethodAttribute : Attribute,IMapsFromAttribute
{
    public Type Src { get; }
    public string Method { get; }
    public MapsUseMethodAttribute(Type src,string method)
    {
        Src = src;
        Method = method;
    }
}
namespace Rop.Mapper.Attributes;
/// <summary>
/// When use in Destiny Property. Use the method from origin 'src'.
/// </summary>
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
namespace Rop.Mapper.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsFromIfAttribute : Attribute,IMapsFromAttribute
{
    public Type Src { get; }
    public string Name { get; }
    public string? Conversor { get; }
    public MapsFromIfAttribute(string name,Type src,string? conversor=null)
    {
        Name = name;
        Src = src;
        Conversor = conversor;
    }
}
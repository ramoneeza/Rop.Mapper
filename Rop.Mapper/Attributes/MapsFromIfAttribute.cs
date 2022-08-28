namespace Rop.Mapper.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsFromIfAttribute : Attribute,IMapsFromAttribute
{
    public Type Src { get; }
    public string Name { get; }
    public MapsFromIfAttribute(string name,Type src)
    {
        Name = name;
        Src = src;
    }
}
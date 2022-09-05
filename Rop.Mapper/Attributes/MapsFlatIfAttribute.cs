namespace Rop.Mapper.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsFlatIfAttribute : Attribute,IMapsIfAttribute
{
    public Type Dst { get; }
    public MapsFlatIfAttribute(Type dst)
    {
        Dst = dst;
    }
}
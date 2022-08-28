namespace Rop.Mapper.Attributes;
/// <summary>
/// Ignore this attribute if destiny is...
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsIgnoreIfAttribute : Attribute,IMapsIfAttribute
{
    public Type Dst { get; }
    public MapsIgnoreIfAttribute(Type dst)
    {
        Dst = dst;
    }
}
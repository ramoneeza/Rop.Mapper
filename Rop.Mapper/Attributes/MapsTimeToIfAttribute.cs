namespace Rop.Mapper.Attributes;

/// <summary>
/// Maps to other name when destiny
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsTimeToIfAttribute : Attribute,IMapsIfAttribute
{
    public string Name { get; }
    public Type Dst { get; }
    public MapsTimeToIfAttribute(string name,Type dst)
    {
        Name = name;
        Dst = dst;
    }
}
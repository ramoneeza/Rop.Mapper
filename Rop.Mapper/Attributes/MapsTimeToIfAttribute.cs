namespace Rop.Mapper.Attributes;

/// <summary>
/// When use in a Time Property, use 'name' property as destiny when destiny class is 'dst'
/// 
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
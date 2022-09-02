namespace Rop.Mapper.Attributes;

/// <summary>
/// Maps to other name when destiny
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple =false)]
public class MapsTimeToAttribute : Attribute,IMapsAttribute
{
    public string Name { get; }
    public MapsTimeToAttribute(string name)
    {
        Name = name;
    }
}
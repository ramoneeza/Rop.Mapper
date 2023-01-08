namespace Rop.Mapper.Attributes;

/// <summary>
/// When use in a time property, use 'name' property as destiny
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
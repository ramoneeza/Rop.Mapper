namespace Rop.Mapper.Attributes;

/// <summary>
/// Maps to other name when destiny
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MapsDateToAttribute : Attribute,IMapsAttribute
{
    public string Name { get; }
    public MapsDateToAttribute(string name)
    {
        Name = name;
    }
}
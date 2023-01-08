namespace Rop.Mapper.Attributes;

/// <summary>
/// Maps a Date Property to 'name' property
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
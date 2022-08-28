namespace Rop.Mapper.Attributes;
/// <summary>
/// When Destiny. Name of the other Property to map.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MapsFromAttribute : Attribute,IMapsAttribute
{
    public string Name { get; }
    public MapsFromAttribute(string name) => Name = name;
}
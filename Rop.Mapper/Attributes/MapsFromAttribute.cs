namespace Rop.Mapper.Attributes;
/// <summary>
/// When declared in Destiny Class. Name of the Property of origin to map.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MapsFromAttribute : Attribute,IMapsAttribute
{
    public string Name { get; }
    public MapsFromAttribute(string name) => Name = name;
}
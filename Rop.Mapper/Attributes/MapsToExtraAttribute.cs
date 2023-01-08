namespace Rop.Mapper.Attributes;

/// <summary>
/// This property maps ALSO to another with other name. Optional conversor is possible.
/// </summary>

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsToExtraAttribute:Attribute,IMapsAttribute
{
    public string Name { get; }
    public string? Conversor { get; }
    public MapsToExtraAttribute(string name, string? conversor)
    {
        Name = name;
        Conversor = conversor;
    }
}
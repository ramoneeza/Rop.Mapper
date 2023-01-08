namespace Rop.Mapper.Attributes;
/// <summary>
/// Ignore always this property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsIgnoreAttribute : Attribute, IMapsAttribute
{
    public MapsIgnoreAttribute() { }
}
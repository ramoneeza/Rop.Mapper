namespace Rop.Mapper.Attributes;
/// <summary>
/// Ignore this property 
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsIgnoreAttribute : Attribute, IMapsAttribute
{
    public MapsIgnoreAttribute() { }
}
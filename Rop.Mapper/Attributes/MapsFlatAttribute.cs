namespace Rop.Mapper.Attributes;
/// <summary>
/// Flat a complex property into his components
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MapsFlatAttribute:Attribute,IMapsAttribute
{
    public MapsFlatAttribute(){}
}
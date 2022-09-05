namespace Rop.Mapper.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MapsFlatAttribute:Attribute,IMapsAttribute
{
    public MapsFlatAttribute(){}
}
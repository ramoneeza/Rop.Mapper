namespace Rop.Mapper.Attributes;
/// <summary>
/// Maps a complex property into his components when destiny is 'dst'
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsFlatIfAttribute : Attribute,IMapsIfAttribute
{
    public Type Dst { get; }
    public MapsFlatIfAttribute(Type dst)
    {
        Dst = dst;
    }
}
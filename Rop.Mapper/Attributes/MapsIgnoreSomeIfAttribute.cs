namespace Rop.Mapper.Attributes;
/// <summary>
/// When use in a class mapping to 'dst'. This properties are ignored.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MapsIgnoreSomeIfAttribute : Attribute, IMapsIfAttribute
{
    public Type Dst { get; }
    public string[] Properties { get; }

    public MapsIgnoreSomeIfAttribute(Type dst,params string[] properties)
    {
        Dst = dst;
        Properties = properties;
    }
}
namespace Rop.Mapper.Attributes;

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
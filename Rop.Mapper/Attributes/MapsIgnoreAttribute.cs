namespace Rop.Mapper.Attributes;
/// <summary>
/// Ignore this property 
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsIgnoreAttribute : Attribute, IMapsAttribute
{
    public MapsIgnoreAttribute() { }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class MapsIgnoreSomeAttribute : Attribute, IMapsAttribute
{
    public string[] Properties { get; }

    public MapsIgnoreSomeAttribute(params string[] properties)
    {
        Properties = properties;
    }
}
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MapsIgnoreSomeIfAttribute : Attribute, IMapsIfAttribute
{
    public Type Dst { get; }
    public string[] Properties { get; }

    public MapsIgnoreSomeIfAttribute(Type dst,params string[] properties)
    {
        Properties = properties;
    }
}


namespace Rop.Mapper.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class MapsIgnoreSomeAttribute : Attribute, IMapsAttribute
{
    public string[] Properties { get; }

    public MapsIgnoreSomeAttribute(params string[] properties)
    {
        Properties = properties;
    }
}
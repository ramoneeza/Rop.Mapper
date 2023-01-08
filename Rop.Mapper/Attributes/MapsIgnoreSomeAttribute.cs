namespace Rop.Mapper.Attributes;
/// <summary>
/// When use in a class. This properties are ignored during mapping.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class MapsIgnoreSomeAttribute : Attribute, IMapsAttribute
{
    public string[] Properties { get; }

    public MapsIgnoreSomeAttribute(params string[] properties)
    {
        Properties = properties;
    }
}
namespace Rop.Mapper.Attributes;

/// <summary>
/// When the destiny property is a string. Format to use when destiny is 'dst'
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsFormatIfAttribute : Attribute,IMapsIfAttribute
{
    public Type Dst { get; }
    public string Format { get; }
    public MapsFormatIfAttribute(string format,Type dst)
    {
        Dst = dst;
        Format = format;
    }
}
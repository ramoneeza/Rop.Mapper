namespace Rop.Mapper.Attributes;
/// <summary>
/// When String. Format to use
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MapsFormatAttribute : Attribute,IMapsAttribute
{
    public string Format{ get; }
    public MapsFormatAttribute(string format)
    {
        Format = format;
    }
}
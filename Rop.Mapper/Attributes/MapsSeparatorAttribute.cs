namespace Rop.Mapper.Attributes;
/// <summary>
/// When a string property to Enumerable conversion occurs, use this separator char.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MapsSeparatorAttribute : Attribute, IMapsAttribute
{
    public string Separator { get; }
    public MapsSeparatorAttribute(string separator)
    {
        Separator = separator;
    }
}
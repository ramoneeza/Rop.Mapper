namespace Rop.Mapper.Attributes;
/// <summary>
/// When String -- Enumerable conversion. Separator char.
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
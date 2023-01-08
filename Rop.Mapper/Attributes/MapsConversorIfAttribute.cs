namespace Rop.Mapper.Attributes;

/// <summary>
/// Conversor to use when maps to certain property destiny
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsConversorIfAttribute : Attribute,IMapsIfAttribute
{
    public string Conversor { get; }
    public Type Dst { get; }
    public MapsConversorIfAttribute(string conversor, Type dst)
    {
        Conversor = conversor;
        Dst = dst;
    }
}
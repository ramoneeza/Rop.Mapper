namespace Rop.Mapper.Attributes;
/// <summary>
/// Use Conversor when maps to destiny
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MapsConversorAttribute : Attribute,IMapsAttribute
{
    public string Conversor { get; }
    public MapsConversorAttribute(string conversor)
    {
        Conversor = conversor;
    }
}
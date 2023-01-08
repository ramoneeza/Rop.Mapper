namespace Rop.Mapper.Attributes;
/// <summary>
/// Conversor to use when maps to any destiny property
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
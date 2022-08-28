namespace Rop.Mapper.Attributes;
/// <summary>
/// When Destiny. Conversor to use when maps from source
/// </summary>
public class MapsFromConversorAttribute : Attribute,IMapsFromAttribute
{
    public string Conversor { get; }
    public Type? Src { get; }
    public MapsFromConversorAttribute(string conversor, Type? src)
    {
        Conversor = conversor;
        Src = src;
    }
}
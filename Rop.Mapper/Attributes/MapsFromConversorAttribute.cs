namespace Rop.Mapper.Attributes;
/// <summary>
/// When used in Destiny Class. Conversor to use when maps from source of type 'dst'
/// </summary>
public class MapsFromConversorAttribute : Attribute,IMapsFromAttribute
{
    public string Conversor { get; }
    public Type Src { get; }
    public MapsFromConversorAttribute(string conversor,Type src)
    {
        Conversor = conversor;
        Src = src;
    }
}
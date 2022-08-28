namespace Rop.Mapper.Attributes;

public interface IMapsIfAttribute:IMapsAttribute
{
    Type Dst { get; }
}
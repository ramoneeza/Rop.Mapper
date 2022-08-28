namespace Rop.Mapper.Attributes;

public interface IMapsFromAttribute:IMapsAttribute
{
    Type Src { get; }
}
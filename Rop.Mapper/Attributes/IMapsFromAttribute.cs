namespace Rop.Mapper.Attributes;
/// <summary>
/// Interface for Maps for Target Mapped Properties
/// </summary>
public interface IMapsFromAttribute:IMapsAttribute
{
    Type Src { get; }
}
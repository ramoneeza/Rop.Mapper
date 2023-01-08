namespace Rop.Mapper.Attributes;
/// <summary>
/// Interface for Conditional Mapped Properties
/// </summary>
public interface IMapsIfAttribute:IMapsAttribute
{
    Type Dst { get; }
}
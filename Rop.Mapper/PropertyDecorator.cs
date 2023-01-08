namespace Rop.Mapper;

public interface IPropertyDecorator
{
    public string? DecoFormat { get; }
    public string? DecoSeparator { get; }
    public object? DecoUseNullValue { get; }
    public string? DecoUseConverter { get; }
}
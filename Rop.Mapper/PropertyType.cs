using Rop.Types;

namespace Rop.Mapper;

public record PropertyType:IPropertyDecorator
{
    public Type Type => TypeProxy.Type;
    public ITypeProxy TypeProxy { get; init; }
    public string? DecoFormat { get; init; }
    public string? DecoSeparator { get; init; }
    public object? DecoUseNullValue { get; init; }
    public string? DecoUseConverter { get; init; }

    public object? GetDefaultValue()
    {
        return DecoUseNullValue ?? (TypeProxy.GetDefaultValue() ?? TypeProxy.Type.GetDefaultValue());
    }
}
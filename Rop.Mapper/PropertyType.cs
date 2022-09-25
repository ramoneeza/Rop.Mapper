using Rop.Types;

namespace Rop.Mapper;

public record PropertyType
{
    public Type Type => TypeProxy.Type;
    public ITypeProxy TypeProxy { get; init; }
    public TypeDecorator TypeDecorator { get; init; }
    public PropertyType(ITypeProxy typeProxy, TypeDecorator typeDecorator)
    {
        TypeProxy = typeProxy;
        TypeDecorator = typeDecorator;
    }

    public object? GetDefaultValue()
    {
        return TypeDecorator.UseNullValue ?? (TypeProxy.GetDefaultValue() ?? TypeProxy.Type.GetDefaultValue());
    }
}
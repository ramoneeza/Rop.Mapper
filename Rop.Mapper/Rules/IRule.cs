namespace Rop.Mapper.Rules;

public interface IRule
{
    int Priority { get; }
    void Apply(Mapper mapper,object src, object dst);

    internal static string? GetConversor(Property? pSrc, Property? pDst, string? conversor = null)
    {
        return conversor ?? (pSrc?.PropertyType.TypeDecorator.UseConverter ?? pDst?.PropertyType.TypeDecorator.UseConverter);
    }
}
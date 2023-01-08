namespace Rop.Mapper.Rules;

/// <summary>
/// Partial Conversion Rule for a Property
/// </summary>
public interface IRule
{
    int Priority { get; }
    void Apply(Mapper mapper,object src, object dst);

    internal static string? GetConversor(Property? pSrc, Property? pDst, string? conversor = null)
    {
        return conversor ?? (pSrc?.PropertyType.DecoUseConverter ?? pDst?.PropertyType.DecoUseConverter);
    }
}
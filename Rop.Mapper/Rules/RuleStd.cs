using System.Collections;
using System.Globalization;
using Rop.Mapper.Attributes;
using Rop.Types;

namespace Rop.Mapper.Rules;

/// <summary>
/// Standard Rule Conversion between properties
/// </summary>
public class RuleStd : IRule
{
    public int Priority => 1;
    internal Property PSrc { get; }
    internal Property PDst { get;}
    private string? Conversor { get; }
    private RuleStd(Property pSrc, Property pDst, string? conversor = null)
    {
        PSrc = pSrc;
        PDst = pDst;
        Conversor =IRule.GetConversor(PSrc,PDst,conversor);
    }
    public static IRule Factory(Property pSrc, Property pDst,string? conversor)
    {
        var rulestd = new RuleStd(pSrc, pDst,conversor);
        if (rulestd.HasErrorAttr())
            return new RuleError("Error by Bad Attribute");
        else
            return rulestd;
    }
    private bool HasErrorAttr()
    {
        return PSrc.HasAtt<MapsErrorAttribute>(out _) || PDst.HasAtt<MapsErrorAttribute>(out _);
    }
    public virtual void Apply(Mapper mapper, object src, object dst)
    {
        var value = PSrc.PropertyProxy.GetValue(src);
        var dstvalue = ConvertValue(mapper, value);
        PDst.PropertyProxy.SetValue(dst, dstvalue);
    }
    protected virtual object? ConvertValue(Mapper mapper, object? valuesrc)
    {
        var typesrc = PSrc.PropertyType;
        var typedst=PDst.PropertyType;
        var converter = (Conversor is not null) ? mapper.GetConverter(Conversor) : mapper.GetConverter(typesrc.Type,typedst.Type);
        return ConversionEngine.ConvertValue(valuesrc, typesrc, typedst, converter);
    }
}
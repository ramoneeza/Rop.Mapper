using Rop.Mapper.Attributes;
using Rop.Types;

namespace Rop.Mapper.Rules;

/// <summary>
/// Mapping over sub property (Property of destiny property)
/// </summary>
public class RuleSubProperty : IRule
{
    public int Priority => 1;
    internal Property PSrc { get; }
    internal Property PDst { get;}
    internal Property PDstSub { get; }

    private string? Conversor { get; }
    
    private RuleSubProperty(Property pSrc, Property pDst,Property pDstSub, string? converter = null)
    {
        PSrc = pSrc;
        PDst = pDst;
        PDstSub = pDstSub;
        Conversor = IRule.GetConversor(PSrc, PDst, converter);
    }

    public static IRule Factory(Property pSrc, Property pDst,Property pDstSub,string? converter=null)
    {
        if (pDst.PropertyType.TypeProxy.TypeCode!=TypeCode.Object) return new RuleError("Only object type for subproperties");
        var rulestd = new RuleSubProperty(pSrc, pDst,pDstSub,converter);
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
        var prefixvalue = PDst.PropertyProxy.GetValue(dst);
        if (prefixvalue is null)
        {
            prefixvalue = Activator.CreateInstance(PDst.PropertyType.Type);
            PDst.PropertyProxy.SetValue(dst,prefixvalue);
        }
        PDstSub.PropertyProxy.SetValue(prefixvalue!, dstvalue);
    }
    protected virtual object? ConvertValue(Mapper mapper, object? valuesrc)
    {
        var typesrc = PSrc.PropertyType;
        var typedst=PDstSub.PropertyType;
        var converter = (Conversor is not null) ? mapper.GetConverter(Conversor) : mapper.GetConverter(typesrc.Type,typedst.Type);
        return ConversionEngine.ConvertValue(valuesrc, typesrc,typedst,converter);
    }
}
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Rules;

public class RuleSubProperty : IRule
{
    internal Property PSrc { get; }
    internal Property PDst { get;}
    internal Property PDstSub { get; }

    private MapsConversorAttribute? ConverterAtt { get; }
    
    private RuleSubProperty(Property pSrc, Property pDst,Property pDstSub)
    {
        PSrc = pSrc;
        PDst = pDst;
        PDstSub = pDstSub;
        var converteratt = PSrc.FindAtt<MapsConversorAttribute>();
        if (converteratt == null) converteratt = PDstSub.FindAtt<MapsConversorAttribute>();
        ConverterAtt = converteratt;
    }

    public static IRule Factory(Property pSrc, Property pDst,Property pDstSub)
    {
        if (pDst.PropertyType.TypeCode!=TypeCode.Object)
            return new RuleError("Only object type for subproperties");
        var rulestd = new RuleSubProperty(pSrc, pDst,pDstSub);
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
        var value = PSrc.PropertyInfo.GetValue(src);
        var dstvalue = ConvertValue(mapper, value);
        var prefixvalue = PDst.PropertyInfo.GetValue(dst);
        if (prefixvalue is null)
        {
            prefixvalue = Activator.CreateInstance(PDst.PropertyType.Type);
            PDst.PropertyInfo.SetValue(dst,prefixvalue);
        }
        PDstSub.PropertyInfo.SetValue(prefixvalue, dstvalue);
    }
    protected virtual object? ConvertValue(Mapper mapper, object? valuesrc)
    {
        var typesrc = PSrc.PropertyType;
        var typedst=PDstSub.PropertyType;
        var converter = (ConverterAtt is not null) ? mapper.GetConverter(ConverterAtt.Conversor) : mapper.GetConverter(typesrc.Type,typedst.Type);
        return ConversionEngine.ConvertValue(valuesrc, typesrc, typedst,converter);
    }
}
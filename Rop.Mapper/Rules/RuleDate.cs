using Microsoft.VisualBasic;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Rules;

public class RuleDate : IRule
{
    public int Priority => 1;
    internal Property PSrc { get; }
    internal Property PDst { get;}
    private RuleDate(Property pSrc, Property pDst)
    {
        PSrc = pSrc;
        PDst = pDst;
    }

    public static IRule Factory(Property pSrc, Property pDst)
    {
        var tsrc = pSrc.PropertyType;
        if (tsrc.Type != typeof(DateTime) && tsrc.Type != typeof(DateOnly) &&tsrc.Type != typeof(DateTime?) && tsrc.Type != typeof(DateOnly?))
        {
            return new RuleError("Error Src must be Date");
        }
        var tdst=pDst.PropertyType;
        if (tdst.Type != typeof(DateTime) && tdst.Type != typeof(DateTime?))
        {
            return new RuleError("Error Dst must be DateTime");
        }
        var rule=new RuleDate(pSrc, pDst);
        if (rule.HasErrorAttr())
            return new RuleError("Error by Bad Attribute");
        else
            return rule;
    }

    private bool HasErrorAttr()
    {
        return PSrc.HasAtt<MapsErrorAttribute>(out _) || PDst.HasAtt<MapsErrorAttribute>(out _);
    }

    public virtual void Apply(Mapper mapper, object src, object dst)
    {
        var value = PSrc.PropertyInfo.GetValue(src);
        DateTime? final = value switch
        {
            DateTime dt => dt,
            DateOnly don => don.ToDateTime(TimeOnly.MinValue),
            _ => null
        };
        PDst.PropertyInfo.SetValue(dst, final);
    }
}
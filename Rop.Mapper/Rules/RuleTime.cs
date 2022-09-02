using Rop.Mapper.Attributes;

namespace Rop.Mapper.Rules;

public class RuleTime : IRule
{
    public int Priority => 2;
    internal Property PSrc { get; }
    internal Property PDst { get;}
    private RuleTime(Property pSrc, Property pDst)
    {
        PSrc = pSrc;
        PDst = pDst;
    }

    public static IRule Factory(Property pSrc, Property pDst)
    {
        var tsrc = pSrc.PropertyType;
        if (tsrc.Type != typeof(TimeSpan) && tsrc.Type != typeof(TimeOnly) &&tsrc.Type != typeof(TimeSpan?) && tsrc.Type != typeof(TimeOnly?))
        {
            return new RuleError("Error Src must be Time");
        }
        var tdst=pDst.PropertyType;
        if (tdst.Type != typeof(DateTime) && tdst.Type != typeof(DateTime?))
        {
            return new RuleError("Error Dst must be DateTime");
        }
        var rule=new RuleTime(pSrc, pDst);
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
        var datetime = PDst.PropertyInfo.GetValue(dst) as DateTime?;
        if (datetime is null || value is null) return;
        DateTime? final = value switch
        {
            TimeSpan ts =>datetime.Value.Date.Add(ts),
            TimeOnly don =>datetime.Value.Date.Add(don.ToTimeSpan()),
            _ => null
        };
        PDst.PropertyInfo.SetValue(dst, final);
    }
}
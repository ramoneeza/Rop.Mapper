using Rop.Mapper.Attributes;
using Rop.Types;

namespace Rop.Mapper.Rules;
/// <summary>
/// Partial Rule conversion about Flating properties
/// </summary>
public class RuleFlat : IRule
{
    public int Priority => 1;
    internal Property PSrc { get; }
    internal Type Dst { get;}
    internal RulesCollection? SubRules { get; }
    public RuleError? Error { get; }

    private RuleFlat(Property pSrc, Type dst)
    {
        PSrc = pSrc;
        Dst = dst;
        var typesrc=pSrc.PropertyType;
        var rules = RulesCollection.Factory(typesrc.Type, dst);
        SubRules = rules;
        if (rules is null)
        {
            Error = new RuleError("Can't make subrules");
        }
        else
        {
            if (!rules.Verify(out var error))
            {
                Error = new RuleError("FlatError" + Environment.NewLine + error);
            }
            else
            {
                Error = rules.GetAll().OfType<RuleError>().FirstOrDefault();
            }
        }
    }

    public static IRule Factory(Property pSrc, Type dst)
    {
        var rulestd = new RuleFlat(pSrc, dst);
        if (rulestd.Error is not null)
            return rulestd.Error;
        else
            return rulestd;
    }
    public virtual void Apply(Mapper mapper, object src, object dst)
    {
        var value = PSrc.PropertyProxy.GetValue(src);
        if (value is null || SubRules is null) return;
        foreach (var rule in SubRules.GetAll())
        {
            rule.Apply(mapper,value,dst);
        }
    }
    
}
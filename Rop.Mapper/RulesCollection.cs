using System.Collections;
using Rop.Mapper.Attributes;
using Rop.Mapper.Rules;


namespace Rop.Mapper;

internal partial class RulesCollection
{
    public Type SrcType { get; }
    public Type DstType { get; }
    private readonly List<IRule> _rules=new List<IRule>();
    private RulesCollection(Type src,Type dst)
    {
        SrcType = src;
        DstType = dst;
        var srcprop = Mapper.GetPropertiesSrc(src);
        var dstprop = Mapper.GetPropertiesDst(dst);
        TraslateAttributes(srcprop, dstprop);
        foreach (var property in srcprop.GetAll())
        {
            var propertyrule = FactoryRule(property,dstprop);
            _rules.Add(propertyrule);
        }
    }

    private void TraslateAttributes(Properties srcprop, Properties dstprop)
    {
        TraslateIf(srcprop, dstprop);
        TraslateFrom(srcprop, dstprop);
        AdjustProperty(srcprop);
    }

    private IRule FactoryRule(Property prop, Properties dstprops)
    {
        if (prop.HasAtt<MapsIgnoreAttribute>(out _)) return Rule.Ignore(prop);
        if (prop.HasAtt<MapsToAttribute>(out var mapstoatt))
        {
            var dstprop = dstprops.Get(mapstoatt!.Name);
            if (dstprop is null) return Rule.Error($"Destiny has not {mapstoatt!.Name} property");
            return Rule.RuleStd(prop, dstprop);
        }
        // Default Name to Name
        var dstpropfinal = dstprops.Get(prop.PropertyName);
        if (dstpropfinal is null) return Rule.Error($"Destiny has not {prop.PropertyName} property");
        return Rule.RuleStd(prop, dstpropfinal);
    }
    public bool Verify()
    {
        return !_rules.Any(r => r is RuleError);
    }

    public static RulesCollection? Factory(Type src, Type dst)
    {
        return new RulesCollection(src,dst);
    }

    public IEnumerable<IRule> GetAll() => _rules;

}
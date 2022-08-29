using System.Collections;
using System.Runtime.InteropServices.ComTypes;
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
        foreach (var property in dstprop.GetAllFrom())
        {
            var propertyrule =FactoryRuleDst(property, srcprop);
            if (propertyrule!=null) _rules.Add(propertyrule);
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
            var name = mapstoatt!.Name;
            if (!name.Contains('.'))
            {

                var dstprop = dstprops.Get(name);
                if (dstprop is null) return Rule.Error($"Destiny has not {name} property");
                return Rule.RuleStd(prop, dstprop);
            }
            else
            {
                var p = name.IndexOf('.');
                var prefix = name[0..p];
                var sufix = name[(p + 1)..];
                var dstprefix = dstprops.Get(prefix);
                if (dstprefix is null) return Rule.Error($"Destiny has not {prefix} prefix property");
                var propsdstsufix = Mapper.GetPropertiesDst(dstprefix.PropertyType.Type);
                if (propsdstsufix is null) return Rule.Error($"Destiny with unknown {prefix} prefix type");
                var dstpropsub = propsdstsufix.Get(sufix);
                if (dstpropsub is null) return Rule.Error($"Destiny with unknown {prefix}.{sufix} sub property");
                return Rule.RuleSubProperty(prop, dstprefix,dstpropsub);
            }
        }
        // Default Name to Name
        var dstpropfinal = dstprops.Get(prop.PropertyName);
        if (dstpropfinal is null) return Rule.Error($"Destiny has not {prop.PropertyName} property");
        return Rule.RuleStd(prop, dstpropfinal);
    }
    private IRule? FactoryRuleDst(Property prop, Properties srcprops)
    {
        if (prop.HasAtt<MapsUseMethodAttribute>(out var useatt) && useatt.Src == srcprops.ClassType)
            return Rule.FactoryUseMethod(prop,DstType, useatt, srcprops);
        else
            return null;

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
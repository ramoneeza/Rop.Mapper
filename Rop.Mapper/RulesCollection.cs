using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
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
            _rules.AddRange(propertyrule);
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

    private IEnumerable<IRule> FactoryRule(Property prop, Properties dstprops)
    {
        if (prop.HasAtt<MapsIgnoreAttribute>(out _))
        {
            yield return Rule.Ignore(prop);
            yield break;
        }
        if (prop.HasAtt<MapsToAttribute>(out var mapstoatt))
        {
            var name = mapstoatt!.Name;
            yield return FactoryRuleMapsTo(name, prop, dstprops);
            yield break;
        }
        var extra = prop.Attributes.OfType<MapsToExtraAttribute>().ToList();
        if (extra.Any())
        {
            foreach (var mapsToExtraAttribute in extra)
            {
                var name = mapsToExtraAttribute.Name;
                var converter = mapsToExtraAttribute.Conversor;
                yield return FactoryRuleMapsTo(name, prop, dstprops,converter);
            }
            yield break;
        }
        // Default Name to Name
        var dstpropfinal = dstprops.Get(prop.PropertyName);
        if (dstpropfinal is null)
             yield return Rule.Error($"Destiny has not {prop.PropertyName} property");
        else
            yield return Rule.RuleStd(prop, dstpropfinal,null);
    }

    private static IRule FactoryRuleMapsTo(string name, Property prop, Properties dstprops,string? converter=null)
    {
        if (!name.Contains('.'))
        {
            var dstprop = dstprops.Get(name);
            if (dstprop is null) return Rule.Error($"Destiny has not {name} property");
            return Rule.RuleStd(prop, dstprop, converter);
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
            return Rule.RuleSubProperty(prop, dstprefix, dstpropsub,converter);
        }
    }

    private IRule? FactoryRuleDst(Property prop, Properties srcprops)
    {
        if (prop.HasAtt<MapsFromUseMethodAttribute>(out var useatt) && useatt.Src == srcprops.ClassType)
            return Rule.FactoryUseMethod(prop,DstType, useatt, srcprops);
        else
            return null;

    }
    public bool Verify(out string error)
    {
        var sb = new StringBuilder();
        foreach (var ruleError in _rules.OfType<RuleError>())
        {
            sb.AppendLine(ruleError.Error);
        }

        error = sb.ToString() ?? "";
        return error == "";
    }

    public static RulesCollection? Factory(Type src, Type dst)
    {
        return new RulesCollection(src,dst);
    }

    public IEnumerable<IRule> GetAll() => _rules;

}
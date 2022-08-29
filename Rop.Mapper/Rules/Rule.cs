using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Rules
{
    public static class Rule
    {
        internal static RuleError Error(string error) => new RuleError(error);
        internal static RuleIgnore Ignore(Property psrc) => new RuleIgnore(psrc);
        internal static IRule RuleStd(Property psrc, Property pdst) => Rop.Mapper.Rules.RuleStd.Factory(psrc,pdst);
        internal static IRule RuleSubProperty(Property psrc, Property pdst,Property psub) => Rop.Mapper.Rules.RuleSubProperty.Factory(psrc, pdst,psub);
        internal static IRule FactoryUseMethod(Property prop,Type dst, MapsUseMethodAttribute useatt, Properties scrprops)
        {
            return RuleUseMethod.Factory(scrprops.ClassType,dst, prop, useatt);
        }
    }
}

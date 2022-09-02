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
        internal static IRule RuleStd(Property psrc, Property pdst,string? conversor) => Rop.Mapper.Rules.RuleStd.Factory(psrc,pdst,conversor);
        internal static IRule RuleSubProperty(Property psrc, Property pdst,Property psub,string? converter=null) => Rop.Mapper.Rules.RuleSubProperty.Factory(psrc, pdst,psub,converter);
        internal static IRule FactoryUseMethod(Property prop,Type dst, MapsFromUseMethodAttribute useatt, Properties scrprops)
        {
            return RuleUseMethod.Factory(scrprops.ClassType,dst, prop, useatt);
        }

        internal static IRule RuleDate(Property psrc, Property pdst) => Rules.RuleDate.Factory(psrc, pdst);
        internal static IRule RuleTime(Property psrc, Property pdst) => Rules.RuleTime.Factory(psrc, pdst);
    }
}

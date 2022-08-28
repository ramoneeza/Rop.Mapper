using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Mapper.Rules
{
    public static class Rule
    {
        internal static RuleError Error(string error) => new RuleError(error);
        internal static RuleIgnore Ignore(Property psrc) => new RuleIgnore(psrc);
        internal static IRule RuleStd(Property psrc, Property pdst) => Rop.Mapper.Rules.RuleStd.Factory(psrc,pdst);
    }
}

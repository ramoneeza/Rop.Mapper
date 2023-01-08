using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;
using Rop.Mapper.Attributes;
using Rop.Types;

namespace Rop.Mapper.Rules;
/// <summary>
/// Alternate Rule Use Method
/// </summary>
public class RuleUseMethod2 : IRule
{
    public int Priority => 2;
    internal PropertyProxy Src { get; }
    internal PropertyProxy Dst { get; }
    private readonly ISimpleMethodProxy _function;
    
    private RuleUseMethod2(PropertyProxy src, PropertyProxy dst,MethodInfo method)
    {
        Src = src;
        Dst = dst;
        _function= SimpleMethodProxy.Get(method);
    }
    public static IRule Factory(PropertyProxy src, PropertyProxy pDst, MethodInfo methodtype)
    {
        var pars = methodtype.GetParameters();
        if (pars.Length !=1)
        {
            return new RuleError("Error by Method bad parameters");
        }
        var rulestd = new RuleUseMethod2(src, pDst, methodtype);
        return rulestd;
    }
    public virtual void Apply(Mapper mapper, object src, object dst)
    {
        var value = Src.GetValue(src);
        var dstvalue = _function.GetValue(dst,value);
        Dst.SetValue(dst,dstvalue);
    }
}
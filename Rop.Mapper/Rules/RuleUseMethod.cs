using System.Linq.Expressions;
using System.Reflection;
using Rop.Mapper.Attributes;
using Rop.Types;

namespace Rop.Mapper.Rules;
/// <summary>
/// Partial mapping rule about UseMethod instead direct mapping
/// </summary>
public class RuleUseMethod : IRule
{
    public int Priority => 2;
    internal Type Src { get; }
    internal Type Dst { get; }
    private readonly ISimpleActionProxy _action;
    private bool MapperParameter { get; }

    private RuleUseMethod(Type src, Type dst,MethodInfo method,bool mpar)
    {
        Src = src;
        Dst = dst;
        _action = SimpleActionProxy.Get(method);
        MapperParameter = mpar;
    }
    public static IRule Factory(Type src,Type dst, Property pDst,MapsFromUseMethodAttribute usematt)
    {
        if (usematt.Src!=src) return new RuleError("Error by Bad Source Type");
        var methodtype = dst.GetMethod(usematt.Method, BindingFlags.Instance | BindingFlags.NonPublic);
        methodtype??= dst.GetMethod(usematt.Method, BindingFlags.Instance | BindingFlags.Public);
        if (methodtype == null)
        {
            return new RuleError("Error by Method not found");
        }

        var pars = methodtype.GetParameters();
        if (pars.Length < 1 || pars.Length > 2)
        {
            return new RuleError("Error by Method bad parameters");
        }

        if (pars.Length == 1)
        {
            if (pars[0].ParameterType != src)
            {
                return new RuleError("Error by Method bad parameter Src");
            }
        }
        if (pars.Length == 2)
        {
            if (pars[0].ParameterType.GetInterface(nameof(IMapper)) == null)
            {
                return new RuleError("Error by Method bad parameter 0 no IMapper");
            }
            if (pars[1].ParameterType != src)
            {
                return new RuleError("Error by Method bad parameter 1 Src");
            }
        }
        var rulestd = new RuleUseMethod(src,pDst.PropertyType.Type,methodtype,pars.Length>1);
        return rulestd;
    }
    public virtual void Apply(Mapper mapper, object src, object dst)
    {
        if (MapperParameter)
        {
            _action.Invoke(dst, mapper, src);
        }
        else
        {
            _action.Invoke(dst, src);
        }
    }
}
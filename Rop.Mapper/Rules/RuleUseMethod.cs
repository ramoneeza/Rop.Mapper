using System.Linq.Expressions;
using System.Reflection;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Rules;

public class RuleUseMethod : IRule
{
    public int Priority => 2;
    internal Type Src { get; }
    internal Type Dst { get; }
    private MethodInfo Method { get; }
    private bool MapperParameter { get; }

    private RuleUseMethod(Type src, Type dst,MethodInfo method,bool mpar)
    {
        Src = src;
        Dst = dst;
        Method = method;
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

    private Action<object,IMapper, object?>? _compiled;

    private void _compile(Mapper mapper, object src, object dst)
    {
        var par0 = Expression.Parameter(typeof(object), "dst");
        var par1 = Expression.Parameter(typeof(IMapper), "mapper");
        var par2 = Expression.Parameter(typeof(object), "src");
        var dstcast=Expression.Convert(par0, dst.GetType());
        var tcast = Method.GetParameters()[(MapperParameter) ? 1 : 0];
        var cast2 = Expression.Convert(par2, tcast.ParameterType);
        var minv = (MapperParameter) ? Expression.Call(dstcast, Method, par1,cast2) : Expression.Call(dstcast,Method, cast2);
        var lambda = Expression.Lambda(minv,par0, par1, par2);
        _compiled =(Action<object,IMapper,object?>)lambda.Compile();
    }
    public virtual void Apply(Mapper mapper, object src, object dst)
    {
        if (_compiled is null) _compile(mapper,src,dst);
        _compiled!(dst,mapper, src);
        
        //if (MapperParameter)
        //{
        //    Method.Invoke(dst, new object[]{mapper, src});
        //}
        //else
        //{
        //    Method.Invoke(dst, new object[] { src });
        //}
    }
}
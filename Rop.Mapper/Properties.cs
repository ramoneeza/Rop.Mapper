using System.Reflection;
using Rop.Mapper.Attributes;

namespace Rop.Mapper;

internal class Properties
{
    public Type ClassType { get; }
    private Dictionary<string, Property> _dic = new(StringComparer.OrdinalIgnoreCase);
    public Properties(Properties other)
    {
        ClassType = other.ClassType;
        foreach (var property in other.GetAll())
        {
            var prop = new Property(property);
            _dic[prop.PropertyName] = prop;
        }
    }

    public Properties(Type classtype,IEnumerable<PropertyInfo> props)
    {
        ClassType = classtype;
        foreach (var propertyInfo in props)
        {
            var prop=new Property(propertyInfo);
            _dic[prop.PropertyName] = prop;
        }
    }

    public static Properties FactorySrc(Type src)
    {
        return new Properties(src,src.GetProperties().Where(p => p.CanRead));
    }
    public static Properties FactoryDst(Type dst)
    {
        return new Properties(dst,dst.GetProperties().Where(p => p.CanWrite));
    }

    public IEnumerable<Property> GetAll() => _dic.Values;

    public Property? Get(string name)
    {
        _dic.TryGetValue(name, out var p);
        return p;
    }
    public Property? GetTo(string name)
    {
        var candidates = GetAll().Where(p => p.MapsTo().Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
        if (candidates.Count != 1) return null;
        return candidates[0];
    }

}
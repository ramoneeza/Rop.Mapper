using System.Collections.Concurrent;
using Rop.Mapper.Converters;
using Rop.Mapper.Rules;

namespace Rop.Mapper;

public static class DefaultMapper
{
    internal static readonly ConcurrentDictionary<string, IConverter> DefaultConvertersDic;
    private static readonly Mapper _defaultmapper;
    public static Dst MapTo<Dst>(this IMappeable item) where Dst : class, new() => _defaultmapper.Map<Dst>(item);
    public static void MapTo<Dst>(this IMappeable item, Dst destiny) where Dst : class => _defaultmapper.Map(item, destiny);
    public static Dst MapTo<Dst>(this IMappeable item,Func<Dst> factorydestiny) => _defaultmapper.Map(item, factorydestiny);

    public static IEnumerable<Dst> MapEnumerable<Dst>(this IEnumerable<IMappeable> item) where Dst : class, new() => _defaultmapper.MapEnumerable<Dst>(item);
    public static IEnumerable<Dst> MapEnumerable<Dst>(this IEnumerable<IMappeable> item,Func<Dst> constructor) where Dst : class, new() => _defaultmapper.MapEnumerable<Dst>(item,constructor);
    
    public static Dst Map<Dst>(object item) where Dst : class, new()
    {
        return _defaultmapper.Map<Dst>(item);
    }
    public static void Map<Dst>(object item, Dst destiny) where Dst : class
    {
        _defaultmapper.Map<Dst>(item, destiny);
    }

    private static void AddDefaultConverter<C>() where C : IConverter,new()
    {
        var converter = new C();
        DefaultConvertersDic[converter.TypeKey()] = converter;
        if (converter is IConverterSymmetric cs) DefaultConvertersDic[cs.InvTypeKey()] = converter;
    }
    private static void AddDefaultConverterByName<C>() where C : IConverter,new()
    {
        var converter = new C();
        DefaultConvertersDic[converter.Name] = converter;
    }

    static DefaultMapper()
    {
        DefaultConvertersDic = new ConcurrentDictionary<string, IConverter>(StringComparer.OrdinalIgnoreCase);
        AddDefaultConverter<DateOnlyConverter>();
        AddDefaultConverter<TimeOnlyConverter>();
        AddDefaultConverter<DateTimeToTimeConverter>();
        _defaultmapper = new Mapper();
    }
}
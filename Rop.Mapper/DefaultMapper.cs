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

    public static IEnumerable<Dst> MapEnumerable<Dst>(this IEnumerable<IMappeable> item) where Dst : class, new() => _defaultmapper.MapEnumerable<Dst>(item);
        
    public static Dst Map<Dst>(object item) where Dst : class, new()
    {
        return _defaultmapper.Map<Dst>(item);
    }
    public static void Map<Dst>(object item, Dst destiny) where Dst : class
    {
        _defaultmapper.Map<Dst>(item, destiny);
    }

    internal static string GetKeyConverter(Type src, Type dst)
    {
        var key = $"{src.Name}|{dst.Name}";
        return key;
    }
    static DefaultMapper()
    {
        DefaultConvertersDic = new ConcurrentDictionary<string, IConverter>(StringComparer.OrdinalIgnoreCase)
        {
            [GetKeyConverter(typeof(DateTime), typeof(DateOnly))] = new DateOnlyConverter(),
            [GetKeyConverter(typeof(DateOnly), typeof(DateTime))] = new DateOnlyConverter(),

            [GetKeyConverter(typeof(TimeSpan), typeof(TimeOnly))] = new TimeOnlyConverter(),
            [GetKeyConverter(typeof(TimeOnly), typeof(TimeSpan))] = new TimeOnlyConverter(),
            [GetKeyConverter(typeof(DateTime), typeof(TimeOnly))] = new DateTimeToTimeConverter(),
        };
        _defaultmapper = new Mapper();
    }
}
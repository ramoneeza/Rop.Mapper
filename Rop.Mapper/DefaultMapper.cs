using System.Collections.Concurrent;
using Rop.Mapper.Converters;
using Rop.Mapper.Rules;

namespace Rop.Mapper;
/// <summary>
/// Default Mapper.
/// Used primary whith classes marked as IMappeable
/// </summary>
public static class DefaultMapper
{
    internal static readonly ConverterDictionary DefaultConvertersDic;
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
        DefaultConvertersDic.RegisterConverterByType<C>();
    }
    private static void AddDefaultConverterByName<C>() where C : IConverter,new()
    {
        DefaultConvertersDic.RegisterConverterByName<C>();
    }

    static DefaultMapper()
    {
        DefaultConvertersDic = new ConverterDictionary();
        AddDefaultConverter<DateOnlyConverter>();
        AddDefaultConverter<DateOnlyConverter2>();
        AddDefaultConverter<TimeOnlyConverter>();
        AddDefaultConverter<TimeOnlyConverter2>();
        AddDefaultConverter<DateTimeToTimeConverter>();
        AddDefaultConverter<DateTimeToTimeConverter2>();
        AddDefaultConverter<BitArrayConverter>();
        _defaultmapper = new Mapper();
    }

    public static void RegistryConverterByName<T>(bool alsobytype=false,bool force=false) where T:IConverter,new()
    {
        _defaultmapper.RegisterConverterByName<T>(alsobytype,force);
    }
    public static void RegistryConverterByType<T>(bool alsobyname=false,bool force=false) where T:IConverter,new()
    {
        _defaultmapper.RegisterConverterByType<T>(alsobyname,force);
    }

}
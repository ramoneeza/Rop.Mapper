using System.Collections;
using System.Security.Cryptography;
using Rop.Mapper.Rules;

namespace Rop.Mapper;

public interface IMapper
{
    Dst Map<Dst>(object item) where Dst:class,new();
    void Map<Dst>(object item,Dst destiny) where Dst:class;
    IEnumerable<Dst> MapEnumerable<Dst>(IEnumerable items) where Dst : class, new();
    IEnumerable<Dst> MapEnumerable<Dst>(IEnumerable items,Func<Dst> constructor) where Dst : class;
    
    IConverter? GetConverter(string convertername);
    IConverter? GetConverter(Type src,Type dst);
    void RegisterConverterByName<C>(bool alsobytype=false,bool force=false) where C:IConverter,new();
    void RegisterConverterByType<C>(bool alsobyname=false, bool force = false) where C : IConverter,new();
}
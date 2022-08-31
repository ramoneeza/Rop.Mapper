using System.Collections;
using System.Security.Cryptography;
using Rop.Mapper.Rules;

namespace Rop.Mapper;

public interface IMapper
{
    Dst Map<Dst>(object item) where Dst:class,new();
    Dst Map<Dst>(object item,Dst destiny) where Dst:class;
    Dst Map<Dst>(object item, Func<Dst> factorydestiny);
    IEnumerable<Dst> MapEnumerable<Dst>(IEnumerable items) where Dst : class, new();
    IEnumerable<Dst> MapEnumerable<Dst>(IEnumerable items,Func<Dst> constructor);
    
    IConverter? GetConverter(string convertername);
    IConverter? GetConverter(Type src,Type dst);
    void RegisterConverterByName<C>(bool alsobytype=false,bool force=false) where C:IConverter,new();
    void RegisterConverterByType<C>(bool alsobyname=false, bool force = false) where C : IConverter,new();
}
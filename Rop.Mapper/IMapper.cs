using System.Collections;
using Rop.Mapper.Rules;

namespace Rop.Mapper;

public interface IMapper
{
    Dst Map<Dst>(object item) where Dst:class,new();
    void Map<Dst>(object item,Dst destiny) where Dst:class;
    IEnumerable<Dst> MapEnumerable<Dst>(IEnumerable items) where Dst : class, new();
    IConverter? GetConverter(string convertername);
    IConverter? GetConverter(Type src,Type dst);
    void RegisterConverter<C>(bool force=false) where C:IConverter,new();
    void RegisterConverter<C>(Type src, Type dst, bool force = false) where C : IConverter,new();
}
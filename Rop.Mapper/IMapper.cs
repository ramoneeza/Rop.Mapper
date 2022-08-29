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
    void RegisterConverter(IConverter converter, bool force=false);
    void RegisterConverter(Type src,Type dst, IConverter converter, bool force=false);
}
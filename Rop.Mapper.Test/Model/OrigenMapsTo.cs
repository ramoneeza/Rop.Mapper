using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Test.Model
{
    public class OriginMapsFrom : IMappeable
    {
        public int Id { get; set; }
        [MapsTo("Nombre")]
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public record DestinyMapsFrom
    {
        [MapsFrom("Id")]
        public int Key { get; init; }
        [MapsFromIf("Name",typeof(OriginMapsFrom))]
        public string Nombre { get; init; }
        [MapsFromIf("Surname", typeof(OriginMapsFrom))]
        public string Apellidos { get; init; }
        public DateTime UpdateDate { get; init; }
        public DateOnly BirthDate { get; init; }
    }
}

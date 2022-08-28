using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Test.Model
{
    public class OriginMapsTo : IMappeable
    {
        [MapsToIf(nameof(DestinyMapsTo.Key),typeof(DestinyMapsTo))]
        public int Id { get; set; }
        [MapsTo("Nombre")]
        public string Name { get; set; }
        [MapsTo("Apellidos")]
        public string Surname { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public record DestinyMapsTo
    {
        public int Key { get; init; }
        public string Nombre { get; init; }
        public string Apellidos { get; init; }
        public DateTime UpdateDate { get; init; }
        public DateOnly BirthDate { get; init; }
    }
}

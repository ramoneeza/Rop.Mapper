using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Test.Model
{
    [MapsIgnoreSome(nameof(Street),nameof(City))]
    [MapsIgnoreSomeIf(typeof(DestinyIgnore2Global),nameof(Phone))]
    public class OriginIgnoreGlobal : IMappeable
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
    }
    public record DestinyIgnore1Global
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Phone { get; set; }
    }

    public record DestinyIgnore2Global
    {
        public string Name { get; init; }
        public string Surname { get; init; }
    }
    
}

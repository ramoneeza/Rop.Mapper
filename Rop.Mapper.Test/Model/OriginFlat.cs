using Rop.Mapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Mapper.Test.Model
{
    public class OriginFlat:IMappeable
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        [MapsFlatIf(typeof(DestinyFlat))]
        public Address Address { get; set; }
    }
    public record DestinyFlat : IMappeable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}

using Rop.Mapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Mapper.Test.Model
{
    public class OriginUseSubProp : IMappeable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [MapsTo("Address.Street")]
        public string Street { get; set; }
        [MapsTo("Address.City")]
        public string City { get; set; }
        [MapsTo("Address.Country")]
        public string Country { get; set; }
        [MapsTo("Address.PostalCode")]
        public string PostalCode { get; set; }
    }

    public record DestinyUseSubProp
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public Address Address { get; set; }
    }
    
}

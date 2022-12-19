using Rop.Mapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Mapper.Test.Model
{
    public class OriginUseMethod2 : IMappeable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [MapsIgnore]
        public string Street { get; set; }
        [MapsIgnore]
        public string City { get; set; }
        [MapsIgnore]
        public string Country { get; set; }
        [MapsIgnore]
        public string PostalCode { get; set; }
    }

    public record DestinyUseMethod2
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        [MapsFromUseMethod(typeof(OriginUseMethod),nameof(_MakeAddress))]
        public Address Address { get; set; }

        private Address _MakeAddress(OriginUseMethod source)
        {
            return new Address()
            {
                City = source.City,
                Country = source.Country,
                PostalCode = source.PostalCode,
                Street = source.Street
            };
        }
    }

    
}

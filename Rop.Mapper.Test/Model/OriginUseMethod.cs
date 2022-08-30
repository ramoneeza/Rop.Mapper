using Rop.Mapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Mapper.Test.Model
{
    public class OriginUseMethod : IMappeable
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

    public record DestinyUseMethod
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        [MapsFromUseMethod(typeof(OriginUseMethod),nameof(_MakeAddress))]
        public Address Address { get; set; }

        private void _MakeAddress(OriginUseMethod source)
        {
            Address = new Address()
            {
                City = source.City,
                Country = source.Country,
                PostalCode = source.PostalCode,
                Street = source.Street
            };
        }
    }

    public record Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}

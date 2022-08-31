using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Test.Model
{
    public class OriginComplex:IMappeable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime UpdateDate { get; set; }
        public byte[] Access { get; set; }
    }

    public record DestinyComplex
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateTime UpdateDate { get; init; }
        public BitArray Access { get; init; } = new BitArray(0);
    }

}

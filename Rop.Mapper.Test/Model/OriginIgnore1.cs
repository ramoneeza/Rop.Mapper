using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Test.Model
{
    public class OriginIgnore1 : IMappeable
    {
        [MapsIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime BirthDate { get; set; }
    }
    public record DestinyIgnore1
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateTime UpdateDate { get; init; }
        public DateOnly BirthDate { get; init; }
    }

    public class OriginIgnore2 : IMappeable
    {
        [MapsIgnoreIf(typeof(DestinyIgnore2A))]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime BirthDate { get; set; }
    }
    public record DestinyIgnore2A
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateTime UpdateDate { get; init; }
        public DateOnly BirthDate { get; init; }
    }
    public record DestinyIgnore2B
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateTime UpdateDate { get; init; }
        public DateOnly BirthDate { get; init; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Test.Model
{
    public class OriginStringToListSeparator:IMappeable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime UpdateDate { get; set; }
        [MapsSeparator("&")]
        public string Groups { get; set; }
    }

    public record DestinyStringToListSeparator
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateTime UpdateDate { get; init; }
        public List<string> Groups { get; init; }
    }
    public record DestinyStringToArraySeparator
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateTime UpdateDate { get; init; }
        public string[] Groups { get; init; }
    }

}

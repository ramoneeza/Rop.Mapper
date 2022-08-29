using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Test.Model
{
    public class OriginStringToList:IMappeable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Groups { get; set; }
    }

    public record DestinyStringToList
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateTime UpdateDate { get; init; }
        public List<string> Groups { get; init; }
    }
    public record DestinyStringToArray
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateTime UpdateDate { get; init; }
        public string[] Groups { get; init; }
    }

}

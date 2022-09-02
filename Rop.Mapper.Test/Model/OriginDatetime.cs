using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Test.Model
{
    public class OriginDateTime:IMappeable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [MapsDateToIf("UpdateDate",typeof(DestinyDateTime))]
        public DateTime UpdateDate { get; set; }
        [MapsTimeToIf("UpdateDate",typeof(DestinyDateTime))]
        public TimeSpan UpdateTime { get; set; }

        [MapsDateToIf("ModifyDate",typeof(DestinyDateTime))]
        public DateOnly? ModifyDate { get; set; }
        [MapsTimeToIf("ModifyDate",typeof(DestinyDateTime))]
        public TimeOnly? ModifyTime { get; set; }

    }
    public record DestinyDateTime
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateTime UpdateDate { get; init; }
        public DateTime ModifyDate { get; init; }
    }

}

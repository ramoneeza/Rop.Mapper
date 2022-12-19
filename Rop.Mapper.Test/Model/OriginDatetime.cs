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

    public record OriginDateTimeCVT : IMappeable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdateDate1 { get; set; }
        public TimeSpan UpdateTime1 { get; set; }
        public DateOnly ModifyDate1 { get; set; }
        public TimeOnly ModifyTime1 { get; set; }

        public DateTime? UpdateDate2 { get; set; }
        public TimeSpan? UpdateTime2 { get; set; }
        public DateOnly? ModifyDate2 { get; set; }
        public TimeOnly? ModifyTime2 { get; set; }

        public DateTime? UpdateDate3 { get; set; }
        public TimeSpan? UpdateTime3 { get; set; }
        public DateOnly? ModifyDate3 { get; set; }
        public TimeOnly? ModifyTime3 { get; set; }


    }
    public record DestinyDateTimeCVT : IMappeable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly UpdateDate1 { get; set; }
        public TimeOnly UpdateTime1 { get; set; }
        public DateTime ModifyDate1 { get; set; }
        public TimeSpan ModifyTime1 { get; set; }

        public DateOnly? UpdateDate2 { get; set; }
        public TimeOnly? UpdateTime2 { get; set; }
        public DateTime? ModifyDate2 { get; set; }
        public TimeSpan? ModifyTime2 { get; set; }

        public DateOnly? UpdateDate3 { get; set; }
        public TimeOnly? UpdateTime3 { get; set; }
        public DateTime? ModifyDate3 { get; set; }
        public TimeSpan? ModifyTime3 { get; set; }
    }


}

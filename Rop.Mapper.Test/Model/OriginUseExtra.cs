using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Attributes;

namespace Rop.Mapper.Test.Model
{
    public record OriginUseExtra:IMappeable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [MapsIgnoreIf(typeof(DestinyUseExtra))] // UseMethod
        public DateTime EnterDate { get; set; }
        [MapsIgnoreIf(typeof(DestinyUseExtra))] // UseMethod
        public TimeSpan EnterTime { get; set; }
    }

    public record DestinyUseExtra   
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }

        [MapsToIf(nameof(OriginUseExtra.EnterDate), typeof(OriginUseExtra),nameof(DtToDateOnlyDt))]
        [MapsToIf(nameof(OriginUseExtra.EnterTime), typeof(OriginUseExtra), nameof(DtToTimeOnly))]
        [MapsFromUseMethod(typeof(OriginUseExtra), nameof(_makeenterdatetime))]
        public DateTime EnterDateTime { get => _enterDateTime; init => _enterDateTime = value; }
        private DateTime _enterDateTime;
        private void _makeenterdatetime(OriginUseExtra origin)
        {
            _enterDateTime = origin.EnterDate.Add(origin.EnterTime);
        }
    }

}

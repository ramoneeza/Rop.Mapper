using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Converters;
using Rop.Mapper.Rules;

namespace Rop.Mapper.Test.Model
{
    public class DtToDateOnlyDt:AbsConverter<DateTime,DateTime>{
        public override DateTime Convert(DateTime value)
        {
            return value.Date;
        }
    }
    public class DtToTimeOnly:AbsConverter<DateTime,TimeSpan>{
        public override TimeSpan Convert(DateTime value)
        {
            return value.TimeOfDay;
        }
    }

    

}

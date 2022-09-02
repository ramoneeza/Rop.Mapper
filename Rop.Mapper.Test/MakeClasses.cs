using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Test.Model;

namespace Rop.Mapper.Test
{
    public static class MakeClasses
    {
        public static (OriginNormal origin, DestinyNormal expected) MakeNormal()
        {
            var origin = new OriginNormal()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                UpdateDate = new DateTime(2022, 1, 4),
                BirthDate = new DateTime(2000, 1, 11)
            };
            var expected = new DestinyNormal()
            {
                Id = origin.Id,
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = origin.UpdateDate,
                BirthDate =DateOnly.FromDateTime(origin.BirthDate)
            };
            return (origin, expected);
        }

        public static (OriginError origin, DestinyError expected) MakeError()
        {
            var origin = new OriginError()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                UpdateDate = new DateTime(2022, 1, 4),
                BirthDate = new DateTime(2000, 1, 11)
            };
            var expected = new DestinyError()
            {
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = origin.UpdateDate,
                BirthDate = DateOnly.FromDateTime(origin.BirthDate)
            };
            return (origin, expected);
        }
        public static (OriginIgnore1 origin, DestinyIgnore1 expected) MakeIgnore1()
        {
            var origin = new OriginIgnore1()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                UpdateDate = new DateTime(2022, 1, 4),
                BirthDate = new DateTime(2000, 1, 11)
            };
            var expected = new DestinyIgnore1()
            {
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = origin.UpdateDate,
                BirthDate = DateOnly.FromDateTime(origin.BirthDate)
            };
            return (origin, expected);
        }
        public static (OriginIgnore2 origin, DestinyIgnore2A expected1, DestinyIgnore2B expected2) MakeIgnore2()
        {
            var origin = new OriginIgnore2()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                UpdateDate = new DateTime(2022, 1, 4),
                BirthDate = new DateTime(2000, 1, 11)
            };
            var expected1 = new DestinyIgnore2A()
            {
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = origin.UpdateDate,
                BirthDate = DateOnly.FromDateTime(origin.BirthDate)
            };
            var expected2 = new DestinyIgnore2B()
            {
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = origin.UpdateDate,
                BirthDate = DateOnly.FromDateTime(origin.BirthDate)
            };
            return (origin, expected1,expected2);
        }
        public static (OriginMapsTo origin, DestinyMapsTo expected) MakeMapsTo()
        {
            var origin = new OriginMapsTo()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                UpdateDate = new DateTime(2022, 1, 4),
                BirthDate = new DateTime(2000, 1, 11)
            };
            var expected = new DestinyMapsTo()
            {
                Key = origin.Id,
                Nombre = origin.Name,
                Apellidos = origin.Surname,
                UpdateDate = origin.UpdateDate,
                BirthDate = DateOnly.FromDateTime(origin.BirthDate)
            };
            return (origin, expected);
        }
        public static (OriginMapsFrom origin, DestinyMapsFrom expected) MakeMapsFrom()
        {
            var origin = new OriginMapsFrom()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                UpdateDate = new DateTime(2022, 1, 4),
                BirthDate = new DateTime(2000, 1, 11)
            };
            var expected = new DestinyMapsFrom()
            {
                Key = origin.Id,
                Nombre = origin.Name,
                Apellidos = origin.Surname,
                UpdateDate = origin.UpdateDate,
                BirthDate = DateOnly.FromDateTime(origin.BirthDate)
            };
            return (origin, expected);
        }
        public static (OriginUseNull origin, DestinyUseNull expected) MakeUseNullValue()
        {
            var origin = new OriginUseNull()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                UpdateDate = new DateTime(2022, 1, 4),
                Age = null,
            };
            var expected = new DestinyUseNull()
            {
                Id = origin.Id,
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = origin.UpdateDate,
                Age = -1
            };
            return (origin, expected);
        }
        public static (OriginStringToList origin, DestinyStringToList expected1,DestinyStringToArray expected2) MakeUseStringToList()
        {
            var origin = new OriginStringToList()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                UpdateDate = new DateTime(2022, 1, 4),
                Groups = "Hello,GoodBye,Sorry",
            };
            var expected1 = new DestinyStringToList()
            {
                Id = origin.Id,
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = origin.UpdateDate,
                Groups    = origin.Groups.Split(',').ToList()
            };
            var expected2 = new DestinyStringToArray()
            {
                Id = origin.Id,
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = origin.UpdateDate,
                Groups = origin.Groups.Split(',')

            };

            return (origin, expected1,expected2);
        }
        public static (OriginUseMethod origin, DestinyUseMethod expected) MakeUseMethod()
        {
            var origin = new OriginUseMethod()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                City = "Almeria",
                PostalCode = "04004",
                Country = "Spain",
                Street = "C/ Mediterraneo"
                
            };
            var expected1 = new DestinyUseMethod()
            {
                Id = origin.Id,
                Name = origin.Name,
                Surname = origin.Surname,
                Address = new (){City  = origin.City,PostalCode = origin.PostalCode,Street = origin.Street,Country = origin.Country}
            };
        
            return (origin, expected1);
        }
        public static (OriginUseSubProp origin, DestinyUseSubProp expected) MakeUseSubProp()
        {
            var origin = new OriginUseSubProp()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                City = "Almeria",
                PostalCode = "04004",
                Country = "Spain",
                Street = "C/ Mediterraneo"

            };
            var expected1 = new DestinyUseSubProp()
            {
                Id = origin.Id,
                Name = origin.Name,
                Surname = origin.Surname,
                Address = new() { City = origin.City, PostalCode = origin.PostalCode, Street = origin.Street, Country = origin.Country }
            };

            return (origin, expected1);
        }
        public static (OriginUseExtra origin, DestinyUseExtra expected) MakeUseExtra()
        {
            DefaultMapper.RegistryConverterByName<DtToDateOnlyDt>(false,true);
            DefaultMapper.RegistryConverterByName<DtToTimeOnly>(false,true);
            var dt = DateTime.Now;
            var origin = new OriginUseExtra()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                EnterDate = dt.Date,
                EnterTime = dt.TimeOfDay
            };
            var expected1 = new DestinyUseExtra()
            {
                Id = origin.Id,
                Name = origin.Name,
                Surname = origin.Surname,
                EnterDateTime    = dt
            };

            return (origin, expected1);
        }
        public static (OriginComplex origin, DestinyComplex expected) MakeComplex()
        {
            
            var raw = new byte[] { 0x4, 0x7, 0x3 };
            var barray = new BitArray(raw);
            
            DefaultMapper.RegistryConverterByName<DtToDateOnlyDt>(false,true);
            DefaultMapper.RegistryConverterByName<DtToTimeOnly>(false,true);
            var dt = DateTime.Now;
            var origin = new OriginComplex()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                UpdateDate = dt,
                Access = raw
            };
            var expected1 = new DestinyComplex()
            {
                Id = origin.Id,
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = dt,
                Access = barray
            };
            return (origin, expected1);
        }
        public static (OriginStringToListSeparator origin, DestinyStringToListSeparator expected1,DestinyStringToArraySeparator expected2) MakeUseStringToListSeparator()
        {
            var origin = new OriginStringToListSeparator()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                UpdateDate = new DateTime(2022, 1, 4),
                Groups = "Hello&GoodBye&Sorry",
            };
            var expected1 = new DestinyStringToListSeparator()
            {
                Id = origin.Id,
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = origin.UpdateDate,
                Groups    = origin.Groups.Split('&').ToList()
            };
            var expected2 = new DestinyStringToArraySeparator()
            {
                Id = origin.Id,
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = origin.UpdateDate,
                Groups = origin.Groups.Split('&')

            };

            return (origin, expected1,expected2);
        }
        public static (OriginDateTime origin, DestinyDateTime expected) MakeDateTime()
        {
            var origin = new OriginDateTime()
            {
                Id = 3,
                Name = "Pepe",
                Surname = "Perez",
                UpdateDate = new DateTime(2022, 1, 4),
                UpdateTime = new TimeSpan(8,45,10),
                ModifyDate = new DateOnly(2022, 6, 5),
                ModifyTime = new TimeOnly(9,30,0)
                
            };
            var expected1 = new DestinyDateTime()
            {
                Id = origin.Id,
                Name = origin.Name,
                Surname = origin.Surname,
                UpdateDate = origin.UpdateDate.Add(origin.UpdateTime),
                ModifyDate = origin.ModifyDate.Value.ToDateTime(origin.ModifyTime.Value)
            };
            
            return (origin, expected1);
        }
    }
    
}

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Rop.Mapper.Converters;
using Rop.Mapper.Rules;

namespace Rop.Mapper
{
    public class Mapper : IMapper
    {
        private static readonly ConcurrentDictionary<(RuntimeTypeHandle src, RuntimeTypeHandle dsc), RulesCollection> _rulesdic = new();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, Properties> _propertiessrc = new();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, Properties> _propertiesdst = new();
        private readonly ConverterDictionary _convertersdic;
        public Dst Map<Dst>(object item) where Dst:class,new()
        {
            var destiny = new Dst();
            Map(item,destiny);
            return destiny;
        }
        private void Map<Dst>(object item,ref Dst destiny)
        {
            var rules = getMapperRules(item.GetType(),destiny!.GetType());
            foreach (var rule in rules.GetAll())
            {
                rule.Apply(this,item, destiny);
            }
        }

        public Dst Map<Dst>(object item, Dst destiny) where Dst:class
        {
            Map(item, ref destiny);
            return destiny;
        }
        public Dst Map<Dst>(object item,Func<Dst> factorydestiny)
        {
            var destiny = factorydestiny();
            Map(item,ref destiny);
            return destiny;
        }

        public IEnumerable<Dst> MapEnumerable<Dst>(IEnumerable items) where Dst : class, new()
        {
            foreach (var item in items)
            {
                yield return Map<Dst>(item);
            }
        }
        public IEnumerable<Dst> MapEnumerable<Dst>(IEnumerable items,Func<Dst> constructor)
        {
            foreach (var item in items)
            {
                yield return Map<Dst>(item,constructor);
            }
        }
        
        private static RulesCollection getMapperRules(Type src,Type dst)
        {
            var key = (src.TypeHandle, dst.TypeHandle);
            if (_rulesdic.TryGetValue(key,out var rules)) return rules;
            rules = RulesCollection.Factory(src,dst);
            _rulesdic[key]=rules;
            return rules;
        }

        internal static Properties GetPropertiesSrc(Type type)
        {
            if (_propertiessrc.TryGetValue(type.TypeHandle, out var properties)) return new(properties);
            properties = Properties.FactorySrc(type);
            _propertiessrc[type.TypeHandle]=properties;
            return new(properties);
        }
        internal static Properties GetPropertiesDst(Type type)
        {
            if (_propertiesdst.TryGetValue(type.TypeHandle, out var properties)) return new(properties);
            properties = Properties.FactoryDst(type);
            _propertiesdst[type.TypeHandle] = properties;
            return new(properties);
        }

        public IConverter? GetConverter(string convertername)=>_convertersdic[convertername];
        public IConverter? GetConverter(Type src,Type dst)
        {
            if (src == dst) return null;
            return _convertersdic[(src.TypeHandle, dst.TypeHandle)];
        }

        public void RegisterConverterByName<C>(bool alsobytype = false, bool force = false) where C : IConverter, new()
        {
            _convertersdic.RegisterConverterByName<C>(alsobytype,force);
        }
        public void RegisterConverterByType<C>(bool alsobyname = false, bool force = false) where C : IConverter, new()
        {
            _convertersdic.RegisterConverterByType<C>(alsobyname,force);
        }
        
        public Mapper()
        {
            _convertersdic = new (DefaultMapper.DefaultConvertersDic);
        }

        public static bool Verify(Type src, Type dst,out string error)
        {
            var rules = getMapperRules(src, dst);
            return rules.Verify(out error);
        }
        public static bool Verify<Src,Dst>(out string error,bool reverse = false)
        {
            if (!Verify(typeof(Src), typeof(Dst), out error))
            {
                error = $"{typeof(Src)} --> {typeof(Dst)}:" + Environment.NewLine + error;
                return false;
            }

            if (reverse && !Verify(typeof(Dst), typeof(Src), out error))
            {
                error = $"{typeof(Src)} <-- {typeof(Dst)}:" + Environment.NewLine + error;
                return false;
            }
            return true;
        }
        public static void VerifyThrow<Src, Dst>(bool reverse = false)
        {
            if (!Verify<Src, Dst>(out var error, reverse))
            {
                error = $"Mapper failed for verifying {typeof(Src)}{(reverse ? "<-->" : "--->")}{typeof(Dst)}" +
                        Environment.NewLine + error;
                throw new Exception(error);
            }
        }
        
        
    }
}
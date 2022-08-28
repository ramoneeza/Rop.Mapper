﻿using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Rop.Mapper.Converters;
using Rop.Mapper.Rules;

namespace Rop.Mapper
{
    public static class DefaultMapper
    {
        internal static readonly ConcurrentDictionary<string, IConverter> DefaultConvertersDic;
        private static readonly Mapper _defaultmapper;
        public static Dst MapTo<Dst>(this IMappeable item) where Dst : class, new() => _defaultmapper.Map<Dst>(item);
        public static void MapTo<Dst>(this IMappeable item, Dst destiny) where Dst : class => _defaultmapper.Map(item, destiny);

        public static IEnumerable<Dst> MapEnumerable<Dst>(this IEnumerable<IMappeable> item) where Dst : class, new() => _defaultmapper.MapEnumerable<Dst>(item);
        
        public static Dst Map<Dst>(object item) where Dst : class, new()
        {
            return _defaultmapper.Map<Dst>(item);
        }
        public static void Map<Dst>(object item, Dst destiny) where Dst : class
        {
            _defaultmapper.Map<Dst>(item, destiny);
        }

        internal static string GetKeyConverter(Type src, Type dst)
        {
            var key = $"{src.Name}|{dst.Name}";
            return key;
        }
        static DefaultMapper()
        {
            DefaultConvertersDic = new ConcurrentDictionary<string, IConverter>(StringComparer.OrdinalIgnoreCase)
            {
                [GetKeyConverter(typeof(DateTime), typeof(DateOnly))] = new DateOnlyConverter(),
                [GetKeyConverter(typeof(DateOnly), typeof(DateTime))] = new DateOnlyConverter(),

                [GetKeyConverter(typeof(TimeSpan), typeof(TimeOnly))] = new TimeOnlyConverter(),
                [GetKeyConverter(typeof(TimeOnly), typeof(TimeSpan))] = new TimeOnlyConverter(),
                [GetKeyConverter(typeof(DateTime), typeof(TimeOnly))] = new DateTimeToTimeConverter(),
            };
            _defaultmapper = new Mapper();
        }
    }
    
    public class Mapper
    {
        private static readonly ConcurrentDictionary<(RuntimeTypeHandle src, RuntimeTypeHandle dsc), RulesCollection> _rulesdic = new();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, Properties> _propertiessrc = new();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, Properties> _propertiesdst = new();

        private readonly ConcurrentDictionary<string, IConverter> _convertersdic;
        public Dst Map<Dst>(object item) where Dst:class,new()
        {
            var destiny = new Dst();
            Map(item,destiny);
            return destiny;
        }
        public void Map<Dst>(object item,Dst destiny) where Dst:class
        {
            var rules = getMapperRules(item.GetType(),destiny.GetType());
            foreach (var rule in rules.GetAll())
            {
                rule.Apply(this,item, destiny);
            }
        }
        public IEnumerable<Dst> MapEnumerable<Dst>(IEnumerable items) where Dst : class, new()
        {
            foreach (var item in items)
            {
                yield return Map<Dst>(item);
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

        public IConverter? GetConverter(string convertername)
        {
            _convertersdic.TryGetValue(convertername, out var value);
            return value;
        }
        public IConverter? GetConverter(Type src,Type dst)
        {
            if (src == dst) return null;
            var key = $"{src.Name}|{dst.Name}";
            return GetConverter(key);
        }

        public void RegisterConverter(IConverter converter, bool force=false)
        {
            RegisterConverter(converter.Name,converter,force);
        }
        private void RegisterConverter(string name,IConverter converter, bool force = false)
        {
            if (_convertersdic.ContainsKey(name) && !force) return;
            _convertersdic[name] = converter;
        }
        public void RegisterConverter(Type src,Type dst, IConverter converter, bool force=false)
        {
            var key = $"{src.Name}|{dst.Name}";
            RegisterConverter(key,converter,force);
        }

        public Mapper()
        {
            _convertersdic = new ConcurrentDictionary<string, IConverter>(DefaultMapper.DefaultConvertersDic,
                StringComparer.OrdinalIgnoreCase);
        }

        public static bool Verify(Type src, Type dst)
        {
            var rules = getMapperRules(src, dst);
            return rules.Verify();
        }
       

    }
}
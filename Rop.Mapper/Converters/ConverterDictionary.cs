using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Rules;

namespace Rop.Mapper.Converters
{
    internal class ConverterDictionary
    {
        private readonly ConcurrentDictionary<string, IConverter> _dic=new ConcurrentDictionary<string, IConverter>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<(RuntimeTypeHandle,RuntimeTypeHandle), IConverter> _dict=new ConcurrentDictionary<(RuntimeTypeHandle,RuntimeTypeHandle),IConverter>();
        public void RegisterConverterByName<C>(bool alsobytype = false, bool force = false) where C : IConverter, new()
        {
            var converter = new C();
            RegisterConverter(converter.Name,converter,force);
            if (alsobytype)
            {
                RegisterConverter(converter.TypeKey(), converter, force);
                if (converter is IConverterSymmetric cs)
                {
                    RegisterConverter(cs.InvTypeKey(), converter, force);
                }
            }
        }

        public void RegisterConverterByType<C>(bool alsobyname = false, bool force = false) where C : IConverter, new()
        {
            var converter = new C();
            RegisterConverter(converter.TypeKey(), converter, force);
            if (converter is IConverterSymmetric cs)
            {
                RegisterConverter(cs.InvTypeKey(), converter, force);
            }
            if (alsobyname)
                RegisterConverter(converter.Name,converter,force);
        }
        private void RegisterConverter(string name,IConverter converter, bool force = false)
        {
            if (_dic.ContainsKey(name) && !force) return;
            _dic[name] = converter;
        }
        private void RegisterConverter((RuntimeTypeHandle,RuntimeTypeHandle) key,IConverter converter, bool force = false)
        {
            if (_dict.ContainsKey(key) && !force) return;
            _dict[key] = converter;
        }

        public IConverter? this[string s]
        {
            get
            {
                _dic.TryGetValue(s,out var converter);
                return converter;
            }
        }
        public IConverter? this[(RuntimeTypeHandle,RuntimeTypeHandle) key]
        {
            get
            {
                _dict.TryGetValue(key,out var converter);
                return converter;
            }
        }

        public ConverterDictionary()
        {
        }

        public ConverterDictionary(ConverterDictionary other)
        {
            _dic=new(other._dic,StringComparer.OrdinalIgnoreCase);
            _dict = new(other._dict);
        }
    }
}

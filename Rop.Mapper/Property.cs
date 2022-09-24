using Rop.Mapper.Attributes;
using Rop.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Mapper
{
    public struct TypeDecorator
    {
        public string? Format;
        public string? Separator;
        public object? UserNullValue;
    }

    public class Property
    {
        protected List<IMapsAttribute> BaseAttributes;
        public IPropertyProxy PropertyProxy { get; }
        public string PropertyName => PropertyProxy.Name;
        public bool CanBeNullable => PropertyProxy.IsNullAllowed;
        public ITypeProxy PropertyType => PropertyProxy.PropertyType;
        private TypeDecorator _typeDecorator;
        public string? Format => _typeDecorator.Format;
        public string? Separator => _typeDecorator.Separator;
        public object? UserNullValue => _typeDecorator.UserNullValue;
        public TypeDecorator TypeDecorator => _typeDecorator;

        public IReadOnlyList<IMapsAttribute> Attributes => BaseAttributes;

        private Property(IPropertyProxy pproxy, IEnumerable<IMapsAttribute>? atts=null)
        {
            PropertyProxy = pproxy;
            atts ??= pproxy.Attributes.OfType<IMapsAttribute>();
            BaseAttributes = new List<IMapsAttribute>(atts);
            AdjustType();
        }
        public Property(PropertyInfo pinfo, IEnumerable<IMapsAttribute>? atts=null):this(Rop.Types.PropertyProxy.Get(pinfo)!,atts)
        {
        }
        public string MapsTo()
        {
            if (HasAtt<MapsToAttribute>(out var matt)) return matt!.Name;
            return PropertyName;
        }
        public Property(Property property) : this(property.PropertyProxy, property.BaseAttributes)
        {
        }
        public T? FindAtt<T>(Func<T, bool> func) where T : IMapsAttribute
        {
            return Attributes.OfType<T>().FirstOrDefault(func);
        }
        public T? FindAtt<T>() where T : IMapsAttribute
        {
            return Attributes.OfType<T>().FirstOrDefault();
        }
        public bool HasAtt<T>(out T? o) where T : IMapsAttribute
        {
            o = FindAtt<T>();
            return o is not null;
        }
        public override string ToString()
        {
            return $"{PropertyName}({PropertyType})";
        }
        internal void Remove(IMapsAttribute att) => BaseAttributes.Remove(att);
        internal void Add(IMapsAttribute att) => BaseAttributes.Add(att);

        internal void AdjustType()
        {
            var formatatt = FindAtt<MapsFormatAttribute>();
            _typeDecorator.Format = formatatt?.Format;
            var separatoratt = FindAtt<MapsSeparatorAttribute>();
            _typeDecorator.Separator = separatoratt?.Separator;
            var defaultvalueatt = FindAtt<MapsUseNullValueAttribute>();
            _typeDecorator.UserNullValue = defaultvalueatt?.Value;
        }
    }
}

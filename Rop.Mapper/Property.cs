using Rop.Mapper.Attributes;
using Rop.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Rules;

namespace Rop.Mapper
{
    /// <summary>
    /// Property attibute under a Rule Collection.
    /// Attibutes are adapted to Src and Dst rules and are not original Attributes
    /// </summary>
    public class Property:IEquatable<Property>
    {
        private readonly List<IMapsAttribute> BaseAttributes;
        public IPropertyProxy PropertyProxy { get; }
        public string PropertyName => PropertyProxy.Name;
        public bool CanBeNullable => PropertyProxy.IsNullAllowed;
        public PropertyType PropertyType { get; private set; }
        
        public IReadOnlyList<IMapsAttribute> Attributes => BaseAttributes;

#pragma warning disable CS8618
        private Property(IPropertyProxy pproxy, IEnumerable<IMapsAttribute>? atts=null)
#pragma warning restore CS8618
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
        internal void Remove(IMapsAttribute att)
        {
            BaseAttributes.Remove(att);
            AdjustType();
        }

        internal void Add(IMapsAttribute att)
        {
            BaseAttributes.Add(att);
            AdjustType();
        }

        internal void AdjustType()
        {
            var formatatt = FindAtt<MapsFormatAttribute>();
            var td = new PropertyType()
            {
                TypeProxy = PropertyProxy.PropertyType,
                DecoFormat = FindAtt<MapsFormatAttribute>()?.Format,
                DecoSeparator = FindAtt<MapsSeparatorAttribute>()?.Separator ?? ",",
                DecoUseNullValue = FindAtt<MapsUseNullValueAttribute>()?.Value,
                DecoUseConverter = FindAtt<MapsConversorAttribute>()?.Conversor
            };
            PropertyType = td;
        }

        public bool Equals(Property? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return BaseAttributes.SequenceEqual(other.BaseAttributes) && PropertyProxy.Equals(other.PropertyProxy) && PropertyType.Equals(other.PropertyType);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Property) obj);
        }

        public override int GetHashCode()
        {
            var hash = 0;
            foreach (var baseAttribute in BaseAttributes)
            {
                hash = hash * 17 + baseAttribute.GetHashCode();
            }
            return HashCode.Combine(hash, PropertyProxy, PropertyType);
        }
    }
}

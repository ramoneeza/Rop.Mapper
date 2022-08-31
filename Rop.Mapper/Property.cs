using System.Diagnostics;
using System.Reflection;
using Rop.Mapper.Attributes;

namespace Rop.Mapper;

public class Property
{
    protected List<IMapsAttribute> BaseAttributes;
    public PropertyInfo PropertyInfo { get; }
    public string PropertyName=>PropertyInfo.Name;
    public bool CanBeNullable { get; }
    public TypeProxy PropertyType { get; private set; }
    public IReadOnlyList<IMapsAttribute> Attributes=>BaseAttributes;

    private Property(PropertyInfo pinfo, IEnumerable<IMapsAttribute> atts)
    {
        PropertyInfo = pinfo;
        BaseAttributes = new List<IMapsAttribute>(atts);
        CanBeNullable = pinfo.CanBeNullable();
        PropertyType = AdjustType();
    }

    public Property(PropertyInfo pinfo):this(pinfo, pinfo.GetCustomAttributes().OfType<IMapsAttribute>())
    {
    }

    public string MapsTo()
    {
        if (HasAtt<MapsToAttribute>(out var matt)) return matt!.Name;
        return PropertyName;
    }
    public Property(Property property) : this(property.PropertyInfo,property.BaseAttributes)
    {
    }

     public T? FindAtt<T>(Func<T, bool> func) where T:IMapsAttribute
    {
        return Attributes.OfType<T>().FirstOrDefault(func);
    }
    public T? FindAtt<T>() where T : IMapsAttribute
    {
        return Attributes.OfType<T>().FirstOrDefault();
    }

    public bool HasAtt<T>(out T? o) where T:IMapsAttribute
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

    internal TypeProxy AdjustType()
    {
        var formatatt = FindAtt<MapsFormatAttribute>();
        var format = formatatt?.Format;
        var separatoratt = FindAtt<MapsSeparatorAttribute>();
        var separator = separatoratt?.Separator;
        var defaultvalueatt = FindAtt<MapsUseNullValueAttribute>();
        var defaultvalue = defaultvalueatt?.Value;
        PropertyType = new TypeProxy(PropertyInfo.PropertyType, CanBeNullable, format, separator, defaultvalue);
        return PropertyType;
    }
}

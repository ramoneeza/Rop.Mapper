namespace Rop.Mapper.Attributes;
/// <summary>
/// Default value to use when Null is going to be mapped.
/// </summary>
public class MapsUseNullValueAttribute : Attribute,IMapsAttribute
{
    public object Value { get; }
    public MapsUseNullValueAttribute(object value)
    {
        Value = value;
            
    }
}
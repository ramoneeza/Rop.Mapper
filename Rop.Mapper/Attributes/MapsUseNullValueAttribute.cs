namespace Rop.Mapper.Attributes;
/// <summary>
/// Default value when is Null
/// </summary>
public class MapsUseNullValueAttribute : Attribute,IMapsAttribute
{
    public object Value { get; }
    public MapsUseNullValueAttribute(object value)
    {
        Value = value;
            
    }
}
namespace Rop.Mapper.Attributes
{

    /// <summary>
    /// This property maps to another with other name
    /// </summary>

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MapsToAttribute:Attribute,IMapsAttribute
    {
        public string Name { get; }
        public MapsToAttribute(string name) => Name = name;
    }
}

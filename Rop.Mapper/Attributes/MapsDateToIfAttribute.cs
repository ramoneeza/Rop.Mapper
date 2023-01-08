namespace Rop.Mapper.Attributes;

/// <summary>
/// Maps a date property to 'name' proerty when destiny type is 'dst'
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsDateToIfAttribute : Attribute,IMapsIfAttribute
{
    public string Name { get; }
    public Type Dst { get; }
    public MapsDateToIfAttribute(string name,Type dst)
    {
        Name = name;
        Dst = dst;
    }
}

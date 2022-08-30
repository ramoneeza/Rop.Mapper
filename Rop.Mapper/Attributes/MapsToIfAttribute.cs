﻿namespace Rop.Mapper.Attributes;
/// <summary>
/// Maps to other name when destiny
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapsToIfAttribute : Attribute,IMapsIfAttribute
{
    public string Name { get; }
    public Type Dst { get; }
    public string? Conversor { get; }
    public MapsToIfAttribute(string name,Type dst,string? conversor=null)
    {
        Name = name;
        Dst = dst;
        Conversor = conversor;
    }
}
﻿namespace Rop.Mapper.Rules;

internal class RuleIgnore : IRule
{
    public Property property { get; }
    public RuleIgnore(Property property)
    {
        this.property = property;
    }
    public void Apply(Mapper mapper, object src, object dst)
    {
        // Do nothing
    }
}
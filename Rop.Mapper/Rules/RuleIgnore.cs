using Rop.Types;

namespace Rop.Mapper.Rules;

/// <summary>
/// Partial Rule conversion about Ignore property
/// </summary>
internal class RuleIgnore : IRule
{
    public int Priority => 0;
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
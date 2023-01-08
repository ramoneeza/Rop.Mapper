namespace Rop.Mapper.Rules;

/// <summary>
/// Partial rule property conversion detected and marked as an Error.
/// </summary>
public class RuleError : IRule
{
    public int Priority => 0;
    public string Error { get; }

    public RuleError(string error)
    {
        Error = error;
    }

    public void Apply(Mapper mapper,object src, object dst)
    {
        throw new Exception(Error);
    }
}
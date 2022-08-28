namespace Rop.Mapper.Rules;

public class RuleError : IRule
{
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
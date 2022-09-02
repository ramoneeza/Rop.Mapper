namespace Rop.Mapper.Rules;

public interface IRule
{
    int Priority { get; }
    void Apply(Mapper mapper,object src, object dst);
}
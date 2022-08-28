namespace Rop.Mapper.Rules;

public interface IRule
{
    void Apply(Mapper mapper,object src, object dst);
}
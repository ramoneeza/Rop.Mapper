namespace Rop.Mapper.Rules;

public interface IConverter
{
    string Name { get; }
    object? Convert(object? value,TypeProxy typesrc,TypeProxy typedst);
}

public interface IConverter<Src, Dst>
{
    Dst? Convert(Src? value);
}
public interface IConverterSym<A,B>
{
    A? Convert(B? value);
    B? Convert(A? value);
}
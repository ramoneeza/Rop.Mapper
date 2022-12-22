using Rop.Types;

namespace Rop.Mapper.Rules;

public interface IConverter
{
    Type AType { get; }
    Type BType { get; }
    string Name { get; }
    bool CanConvertNull { get; }
    object? Convert(object? value,PropertyType typesrc,PropertyType typedst);
    public (RuntimeTypeHandle,RuntimeTypeHandle) TypeKey()=>(AType.TypeHandle,BType.TypeHandle);
}

public interface IConverter<A, B>:IConverter
{
    B? Convert(A? value);
}

public interface IConverterSymmetric : IConverter
{
    public (RuntimeTypeHandle,RuntimeTypeHandle) InvTypeKey()=>(BType.TypeHandle,AType.TypeHandle);
}
public interface IConverterSymmetric<A,B>:IConverter<A,B>,IConverterSymmetric
{
    A? Convert(B? value);
}
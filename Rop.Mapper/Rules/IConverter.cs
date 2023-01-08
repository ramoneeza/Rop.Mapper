using Rop.Types;

namespace Rop.Mapper.Rules;



/// <summary>
/// Base interface for converting from AType to BType properties
/// </summary>
public interface IConverter
{
    Type AType { get; }
    Type BType { get; }
    string Name { get; }
    bool CanConvertNull { get; }
    object? Convert(object? value,PropertyType typesrc,PropertyType typedst);
    public (RuntimeTypeHandle,RuntimeTypeHandle) TypeKey()=>(AType.TypeHandle,BType.TypeHandle);
}

/// <summary>
/// Generic Interface for converting from A type to B type
/// </summary>
/// <typeparam name="A">Origin Type</typeparam>
/// <typeparam name="B">Destiny Type</typeparam>
public interface IConverter<A, B>:IConverter
{
    B? Convert(A? value);
}
/// <summary>
/// Base Interface for a Symmetric converter A<-->B
/// </summary>
public interface IConverterSymmetric : IConverter
{
    public (RuntimeTypeHandle,RuntimeTypeHandle) InvTypeKey()=>(BType.TypeHandle,AType.TypeHandle);
}
/// <summary>
/// Generic Interface for a Symmetric converter A<-->B
/// </summary>
/// <typeparam name="A"></typeparam>
/// <typeparam name="B"></typeparam>
public interface IConverterSymmetric<A,B>:IConverter<A,B>,IConverterSymmetric
{
    A? Convert(B? value);
}

# Rop.Mapper

Features
--------

Rop.Mapper is an unobstrusive alternative to AutoMapper.
Rop.Mapper allows maps entities using custom Attributes.
Rop.Mapper works as Singleton or as Dependency Injection.

To easy use as Singleton, optionaly, mappeables entities can be signaled as IMappeable.


## Direct Use

For entities signaled as IMappeable you can use Extensions Methods

```csharp
public static Dst MapTo<Dst>(this IMappeable item) where Dst : class, new()
public static void MapTo<Dst>(this IMappeable item, Dst destiny) where Dst : class
```

For entities not signaled, you can use static class DefaultMapper

```csharp
DefaultMapper.Map<Dst>(object item);
DefaultMapper.Map<Dst>(object item, Dst destiny)
```

## As Dependency Injection

A mapper with custom converters can be instantiated.

## Verify
You can verify right conversion rules between entities before use
```csharp
Mapper.Verify<Src,Dst>();
```

## Maps Attributes

```csharp
// Universal
[MapsTo(string name)]
[MapsUseNullValue(object value)]
[MapsIgnore()]
[MapsFormat(string format)]
[MapsConversor(string conversor)]
[MapsSeparator(string separator)]
// When destination entity is of certain type
[MapsToIf(string name,Type dst)]
[MapsIgnoreIf(Type dst)]
[MapsFormatIf(string format,Type dst)]
[MapsConversorIf(string conversor,Type dst)]
// When origin entity is if certain type
[MapsFrom(string name,Type src)]
[MapsFromConversor(string conversor,Type src)]
```

 ------
 (C)2022 Ramón Ordiales Plaza

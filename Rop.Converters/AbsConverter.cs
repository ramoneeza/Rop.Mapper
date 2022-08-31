namespace Rop.Converters
{
    public abstract class Converter<A,B>:IConverter<A,B>
    {
        public Type AType { get; }
        public Type BType { get; }
        public string Key { get; }
        public string Name { get; }
        public abstract Func<A,B> Convert { get; }
        protected Converter(string name=null)
        {
            AType = typeof(A);
            BType = typeof(B);
            Key = (this as IConverter).TypeKey();
            Name = name??this.GetType().Name;
        }
    }
}

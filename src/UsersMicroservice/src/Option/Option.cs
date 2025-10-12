namespace Option
{

    /// <summary>
    /// Implementation of Scala's Option type in C#
    /// https://github.com/scala/scala/blob/v2.13.16/src/library/scala/Option.scala#L144
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract record Option<T>
    {
        public abstract bool IsEmpty { get; }
        public abstract T Get();
        
        public sealed record Some(T Value) : Option<T> 
        {
            public override bool IsEmpty=> false;
            public override T Get() => Value;
        }
    
        public sealed record None : Option<T> 
        {
            public override bool IsEmpty => true;
            public override T Get() => throw new InvalidOperationException("Cannot call Get() on None");
        }
        
    }
    
    
    /// <summary>
    /// Like scala's companion object. Makes it possible to instantiate without the new keyword
    /// <code>
    /// Option<string> someName = Option.Some("mathi");
    /// Option<string> noName = Option.None<string>();
    /// Option<string> maybeName = Option.of(name);
    /// </code>
    /// </summary>
    public static class Option
    {
        public static Option<T> Some<T>(T value) where T : notnull =>
            new Option<T>.Some(value);

        public static Option<T> None<T>() where T : notnull =>
            new Option<T>.None();
        
        public static Option<T> Of<T>(T? value) where T : class => 
            value is null ? None<T>() : Some<T>(value);
    }
    
    
}
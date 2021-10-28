namespace streams.MicroServiceA.Streams
{
    using streams.Core;

    internal record FooEvent(int Value) : IEvent;
}

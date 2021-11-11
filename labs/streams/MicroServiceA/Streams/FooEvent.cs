namespace streams.MicroServiceA.Streams
{
    using nflow.core.Flow;
    using streams.Core;

    internal record FooEvent(int Value) : IEvent;
}

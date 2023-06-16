namespace Flow.Reactive.IPC.Console.ApplicationB.MicroServiceB.Streams.Public
{
    using Flow.Reactive.Streams.Persisted;

    public class AStream : PublicPersistedStream<A>
    {
        public override A InitialState { get; } = new A();
    }

    public class A : PersistedStreamData
    { }
}

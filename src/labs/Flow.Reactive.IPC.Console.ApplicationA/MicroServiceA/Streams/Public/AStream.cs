namespace Flow.Reactive.IPC.Console.ApplicationA.MicroServiceA.Streams.Public
{
    using Flow.Reactive.Streams.Persisted;

    public class AStream : PublicPersistedStream<A>
    {
        public override A InitialState { get; } = new A();
    }

    public class A : PersistedStreamData
    { }

}

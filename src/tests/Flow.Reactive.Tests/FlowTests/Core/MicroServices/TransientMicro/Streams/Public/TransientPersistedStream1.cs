namespace Flow.Reactive.Tests.FlowTests.Core.MicroServices.TransientMicro.Streams.Public
{
    using Flow.Reactive.Streams.Persisted;

    public class TransientPersistedStream1 : PublicPersistedStream<TransientPersistedStream1Data>
    {
        public override TransientPersistedStream1Data InitialState { get; } = new();
    }

    public class TransientPersistedStream1Data : PersistedStreamData
    {
        public int Count { get; set; }
    }
}

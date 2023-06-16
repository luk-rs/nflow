namespace Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.Streams.Public
{
    using Flow.Reactive.Streams.Persisted;

    public class PersistedStream1 : PublicPersistedStream<PersistedStream1Data>
    {
        public override PersistedStream1Data InitialState { get; } = new();
    }

    public class PersistedStream1Data : PersistedStreamData
    {
        public int Count { get; set; }
    }
}

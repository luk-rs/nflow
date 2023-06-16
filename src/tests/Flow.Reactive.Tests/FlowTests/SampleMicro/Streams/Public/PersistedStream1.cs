namespace Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public
{
    using Flow.Reactive.Streams.Persisted;

    public class PersistedStream1 : PublicPersistedStream<PersistedStreamData1>
    {
        public override PersistedStreamData1 InitialState => new();
    }

    public class PersistedStreamData1 : PersistedStreamData
    {
        public int UpdateCount { get; set; }
    }
}

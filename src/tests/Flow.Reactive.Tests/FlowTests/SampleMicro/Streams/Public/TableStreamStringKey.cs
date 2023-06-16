namespace Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public
{
    using Flow.Reactive.Streams.Persisted;
    using Flow.Reactive.Streams.Persisted.Table;

    public class TableStreamStringKey : PublicPersistedStream<TableStringKey>
    {
        public override TableStringKey InitialState => new();
    }

    public class TableStringKey : TablePersistedData<string, RecordData>
    { }
}


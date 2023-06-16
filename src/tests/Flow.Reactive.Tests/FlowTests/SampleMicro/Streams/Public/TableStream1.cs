namespace Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public
{
    using Flow.Reactive.Streams.Persisted;
    using Flow.Reactive.Streams.Persisted.Table;

    public class TableStream1 : PublicPersistedStream<Table>
    {
        public override Table InitialState => new();
    }

    public class Table : TablePersistedData<int, RecordData>
    { }

    public class RecordData
    {
        public RecordData(string value) => Value = value;

        public string Value { get; }
    }
}


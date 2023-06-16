namespace Flow.Reactive.Tests.FlowTests.Reconnect.Streams.Public
{
    using Flow.Reactive.Streams.Persisted;
    using Flow.Reactive.Streams.Persisted.Table;

    public class ReconnectableTableStream : PublicPersistedStream<ReconnectableTable>
    {
        public override ReconnectableTable InitialState
        {
            get => new ReconnectableTable();
        }
    }

    public class ReconnectableTable : TablePersistedData<int, RecordData>
    { }

    public class RecordData
    {
        public RecordData(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}

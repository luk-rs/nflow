namespace Flow.Reactive.Tests.FlowTests.Reconnect.Streams.Public
{
    using Flow.Reactive.Streams.Persisted;

    public class CollectionStream : PublicPersistedStream<IntCollection>
    {
        public override IntCollection InitialState => new ();
    }

    public class IntCollection : PersistedStreamDataCollection<int>
    {

    }
}

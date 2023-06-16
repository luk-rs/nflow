namespace Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.Streams.Private
{
    using Flow.Reactive.Streams.Persisted;

    public class PrivatePersistedStream1 : PersistedStream<PrivatePersistedStream1Data>
    {
        public override PrivatePersistedStream1Data InitialState { get; } = new();
    }

    public class PrivatePersistedStream1Data : PersistedStreamData
    { }
}

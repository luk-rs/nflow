namespace Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.Streams.Public
{
    using Flow.Reactive.Streams.Ephemeral;

    public class EventStream1 : PublicEventsStream<EventStream1Data>
    {
    }

    public class EventStream1Data : StreamData
    {
        public EventStream1Data(int count) =>
            Count = count;

        public int Count { get; }
    }
}

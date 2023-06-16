namespace Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public
{
    using Flow.Reactive.Streams.Ephemeral;

    public class EventStream1 : EventsStream<EventStreamData1>
    {
        public override bool Public => true;
    }

    public class EventStreamData1 : StreamData
    { }
}

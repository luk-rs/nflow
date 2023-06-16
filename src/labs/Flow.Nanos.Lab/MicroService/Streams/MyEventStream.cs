namespace Flow.Nanos.Lab.MicroService.Streams
{
    using Flow.Reactive.Streams.Ephemeral;

    public class MyEventStream : PublicEventsStream<MyEventPayload>
    {}

    public class MyEventPayload : StreamData
    {
        public MyEventPayload(int eventValue) => EventValue = eventValue;

        public int EventValue { get; }
    }
}

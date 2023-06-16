namespace Flow.Nanos.Lab.MicroService.Streams
{
    using Flow.Reactive.Streams.Persisted;

    public class MyPersistedStream : PublicPersistedStream<MyPersistedStreamPayload>
    {
        public override MyPersistedStreamPayload InitialState => new();
    }

    public class MyPersistedStreamPayload : PersistedStreamData
    {
        public int Value { get; set; }   
    }
}

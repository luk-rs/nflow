namespace Flow.Reactive.Playground.SharedKernel
{

    using Streams.Persisted.Json;


    public class Integer : JsonStreamData
    {
        public int Value { get; set; }
    }
}

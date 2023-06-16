namespace Flow.Reactive.Playground.MicroServices.Processing.Streams
{
    using SharedKernel;
    using Reactive.Streams.Persisted.Json;

    public class IntegersStream : JsonStream<Integers>
    {

        public override bool Public => true;

    }

    public class Integers : JsonStreamDataCollection<Integer>
    {
    }
}
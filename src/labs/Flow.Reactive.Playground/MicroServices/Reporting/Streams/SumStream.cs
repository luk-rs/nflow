namespace Flow.Reactive.Playground.MicroServices.Reporting.Streams
{

    using Reactive.Streams.Persisted;


    public class SumStream : PersistedStream<Sum>
    {

        public override Sum InitialState => new Sum {Total = 0};
        public override bool Public => true;

    }


    public class Sum : PersistedStreamData
    {

        public int Total { get; set; }

    }

}
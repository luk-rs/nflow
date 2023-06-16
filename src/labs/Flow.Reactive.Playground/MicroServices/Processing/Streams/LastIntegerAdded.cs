namespace Flow.Reactive.Playground.MicroServices.Processing.Streams
{

    using Reactive.Streams.Persisted;


    public class LastIntegerAddedStream : PersistedStream<LastIntegerAdded>
    {

        public override LastIntegerAdded InitialState => new LastIntegerAdded(int.MinValue);
        public override bool Public => true;

    }


    public class LastIntegerAdded : PersistedStreamData
    {

        public LastIntegerAdded(int value)
        {
            Value = value;
        }

        public int Value { get; set; }

        public string PrintValue => Value == int.MinValue ? "No values yet" : Value.ToString();

    }

}
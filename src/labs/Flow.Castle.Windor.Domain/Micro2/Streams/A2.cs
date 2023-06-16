namespace Flow.Castle.Windsor.Domain.Micro2.Streams
{

    using Reactive.Streams.Persisted;


    public class A2 : PersistedStream<B2>
    {

        public override bool Public => true;
        public override B2 InitialState => new B2();

    }
    public class B2 : PersistedStreamData
    {

        public int Control { get; set; } = 0;

    }

    public class C2 : PersistedStream<D2>
    {

        public override bool Public => true;
        public override D2 InitialState => new D2();

    }
    public class D2 : PersistedStreamData
    {

        public int Control { get; set; } = 0;

    }
}
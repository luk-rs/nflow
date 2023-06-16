namespace Flow.Castle.Windsor.Domain.Micro.Streams
{

    using Reactive.Streams.Persisted;


    public class A : PersistedStream<B>
    {

        public override bool Public => true;
        public override B InitialState => new B();

    }
    public class B : PersistedStreamData
    {

        public int Control { get; set; } = 0;

    }

    public class C : PersistedStream<D>
    {

        public override bool Public => true;
        public override D InitialState => new D();

    }
    public class D : PersistedStreamData
    {

        public int Control { get; set; } = 0;

    }
}
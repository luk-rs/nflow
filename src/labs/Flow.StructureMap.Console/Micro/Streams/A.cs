namespace Flow.StructureMap.Console.Micro.Streams
{

    using Reactive.Streams.Persisted;


    internal class A : PersistedStream<B>
    {

        public override bool Public => true;
        public override B InitialState => new B();

    }
    internal class B : PersistedStreamData
    {

        public int Control { get; set; } = 0;

    }

}
namespace FlowIPC.Console.ApplicationA.MicroServiceA.Streams.Public
{
    using Flow.Reactive.Streams.Ephemeral;

    public class SomethingHappenedStream : EventsStream<SomethingHappened>
    {
        public override bool Public => true;
    }

    public class SomethingHappened : StreamData
    {

    }
}

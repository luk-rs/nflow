namespace Flow.Reactive.IPC.Console.ApplicationB.MicroServiceB.Streams.Public
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

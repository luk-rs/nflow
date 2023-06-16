namespace Flow.Reactive.IPC.Console.ApplicationB.MicroServiceB.NanoServices
{
    using System;
    using System.Reactive;
    using Flow.Reactive.IPC.Console.ApplicationB.MicroServiceB.Streams.Public;
    using Flow.Reactive.Services;

    public class SomethingHappenedListener : EventListenerNano<SomethingHappened>
    {
        public override IObservable<Unit> Connect() =>
            Listen
                .PerformAction(x => Console.WriteLine("Something Happened was just listened"));
    }
}

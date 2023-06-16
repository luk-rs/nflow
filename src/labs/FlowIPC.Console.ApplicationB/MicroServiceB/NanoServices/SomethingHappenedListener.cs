namespace FlowIPC.Console.ApplicationB.MicroServiceB.NanoServices
{
    using System;
    using System.Reactive;
    using Flow.Reactive.Services;
    using FlowIPC.Console.ApplicationB.MicroServiceB.Streams.Public;

    public class SomethingHappenedListener : EventListenerNano<SomethingHappened>
    {
        public override IObservable<Unit> Connect() =>
            Listen
                .PerformAction(x => Console.WriteLine("Something Happened was just listened"));
    }
}

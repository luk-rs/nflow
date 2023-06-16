namespace FlowIPC.Console.ApplicationB.MicroServiceB.NanoServices
{
    using System;
    using System.Reactive;
    using Flow.Reactive.Services;
    using FlowIPC.Console.ApplicationB.MicroServiceB.Streams.Public;

    public class PersistedValueReporter : QueryNano<PersistedValue>
    {
        public override IObservable<Unit> Connect() =>
            Query
                .PerformAction(x => Console.WriteLine($"Persisted Value: {x.Value}"));
    }
}

namespace Flow.Reactive.IPC.Console.ApplicationB.MicroServiceB.NanoServices
{
    using System;
    using System.Reactive;
    using Flow.Reactive.IPC.Console.ApplicationB.MicroServiceB.Streams.Public;
    using Flow.Reactive.Services;

    public class PersistedValueReporter : QueryNano<PersistedValue>
    {
        public override IObservable<Unit> Connect() =>
            Query
                .PerformAction(x => Console.WriteLine($"Persisted Value: {x.Value}"));
    }
}

namespace Flow.Nanos.Lab.MicroService.NanoServices
{
    using Flow.Nanos.Lab.MicroService.Streams;
    using Flow.Reactive.Services;
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;

    public class Updater : QueryNano<MyPersistedStreamPayload>
    { 
        public override IObservable<Unit> Connect() =>
            Query
               .AndListen<MyPersistedStreamPayload, MyEventPayload>(this)
               .Do(persistedStream => Console.WriteLine($"Updating Value {persistedStream.Value}"))
               .Select(_ => Unit.Default);

        //public override IObservable<Unit> Connect() =>
        //     Query
        //        .AndListenWhen<MyPersistedStreamPayload, MyEventPayload>(this,
        //            (persistedValue, @event) => persistedValue.Value == @event.EventValue)
        //        .Do(persistedStream => Console.WriteLine($"Updating Value {persistedStream.Value}"))
        //        .Select(_ => Unit.Default);
    }
}

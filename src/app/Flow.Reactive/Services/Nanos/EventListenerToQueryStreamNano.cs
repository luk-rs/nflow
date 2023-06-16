namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Streams.Ephemeral;
    using Streams.Persisted;


    public abstract class EventListenerToQueryStreamNano<TEventStreamData, TStreamData> : Nano
        where TEventStreamData : StreamData
        where TStreamData : PersistedStreamData
    {
        public override int StartOrder => 500;

        public override IObservable<Unit> Connect()
            => Listen<TEventStreamData>()
                .Where(queryData => Filter(queryData))
                .Select(eventData => Update<TStreamData>(x => Updater(eventData)(x)))
                .Concat();

        protected abstract Func<TEventStreamData, Action<TStreamData>> Updater { get; }

        protected virtual Predicate<TEventStreamData> Filter { get; } = _ => true;
    }
}
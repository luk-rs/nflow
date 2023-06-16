namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Streams.Ephemeral;


    public abstract class EventListenerToEventStreamNano<TEventStreamData, TStreamData> : Nano
        where TEventStreamData : StreamData
        where TStreamData : StreamData
    {
        public override int StartOrder => 500;

        public override IObservable<Unit> Connect()
            => Listen<TEventStreamData>()
                .Where(queryData => Filter(queryData))
                .Select(@event => Notify(CreateEvent(@event)))
                .Concat();

        protected abstract Func<TEventStreamData, TStreamData> CreateEvent { get; }

        protected virtual Predicate<TEventStreamData> Filter { get; } = _ => true;
    }
}
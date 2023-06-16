namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Streams.Ephemeral;
    using Streams.Persisted;


    public abstract class QueryStreamToEventStreamNano<TQueryStreamData, TStreamData> : Nano
       where TQueryStreamData : PersistedStreamData
       where TStreamData : StreamData
    {
        public override int StartOrder => 2000;

        public override IObservable<Unit> Connect()
            => Query<TQueryStreamData>()
                .Where(queryData => Filter(queryData))
                .Select(queryData => Notify(CreateEvent(queryData)))
                .Concat();

        protected abstract Func<TQueryStreamData, TStreamData> CreateEvent { get; }

        protected virtual Predicate<TQueryStreamData> Filter { get; } = _ => true;
    }
}
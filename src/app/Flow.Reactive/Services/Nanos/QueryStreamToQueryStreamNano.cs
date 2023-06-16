namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Streams.Persisted;


    public abstract class QueryStreamToQueryStreamNano<TQueryStreamData, TStreamData> : Nano
       where TQueryStreamData : PersistedStreamData
       where TStreamData : PersistedStreamData
    {
        public override int StartOrder => 2000;

        public override IObservable<Unit> Connect()
            => Query<TQueryStreamData>()
                .Where(queryData => Filter(queryData))
                .Select(queryData => Update<TStreamData>(x => Updater(queryData)(x)))
                .Concat();

        protected abstract Func<TQueryStreamData, Action<TStreamData>> Updater { get; }

        protected virtual Predicate<TQueryStreamData> Filter { get; } = _ => true;
    }
}
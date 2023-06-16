namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Streams.Persisted;


    public abstract class StarterAsyncNano<T, TStreamData> : Nano where TStreamData : PersistedStreamData
    {
        public override int StartOrder => 5000;

        public override IObservable<Unit> Connect()
            => Observable
                .FromAsync(InitialValue)
                .Select(initialValue => Update<TStreamData>(x => Updater(initialValue)(x)))
                .Concat();

        protected abstract Task<T> InitialValue();

        protected abstract Func<T, Action<TStreamData>> Updater { get; }
    }
}
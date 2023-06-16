namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Streams.Persisted;


    public abstract class StarterNano<T, TStreamData> : Nano where TStreamData : PersistedStreamData
    {
        public override int StartOrder => 5000;

        public override IObservable<Unit> Connect()
            => Observable
                .Return(InitialValue)
                .Select(initialValue => Update<TStreamData>(x => Updater(initialValue)(x)))
                .Concat();

        protected abstract T InitialValue { get; }

        protected abstract Func<T, Action<TStreamData>> Updater { get; }
    }
}
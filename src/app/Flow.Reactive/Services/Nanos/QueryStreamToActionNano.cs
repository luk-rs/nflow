namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Streams.Persisted;


    public abstract class QueryStreamToActionNano<TStreamData> : Nano
            where TStreamData : PersistedStreamData
    {

        public override int StartOrder => 2000;

        public override IObservable<Unit> Connect() => Query<TStreamData>()
                                                      .Do(x => Action(x))
                                                      .Select(_ => Unit.Default);

        protected abstract Action<TStreamData> Action { get; }

    }

}
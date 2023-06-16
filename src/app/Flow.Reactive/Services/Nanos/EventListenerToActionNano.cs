namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Streams.Ephemeral;


    public abstract class EventListenerToActionNano<TStreamData> : Nano
        where TStreamData : StreamData
    {
        public override int StartOrder => 500;

        public override IObservable<Unit> Connect()
            => Listen<TStreamData>()
                .Do(x => Action(x))
                .Select(_ => Unit.Default);

        protected abstract Action<TStreamData> Action { get; }
    }
}
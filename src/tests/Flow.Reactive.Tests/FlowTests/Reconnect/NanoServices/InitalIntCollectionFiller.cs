namespace Flow.Reactive.Tests.FlowTests.Reconnect.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.Reconnect.Streams.Public;
    using System;
    using System.Reactive;
    using System.Reactive.Linq;

    public class InitalIntCollectionFiller : TriggerNano<int>
    {
        protected override IObservable<int> Trigger => Observable.Return(1);

        public override IObservable<Unit> Connect() =>
            Trigger
            .Update<int, IntCollection>(this, (value, stream) => stream.Add(value));
    }
}

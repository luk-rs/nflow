namespace Flow.Reactive.Playground.MicroServices.Processing.NanoServices
{

    using Streams;
    using System;
    using System.Linq;
    using Flow.Reactive.Services;
    using System.Reactive;

    public class LastIntegerObserver : QueryNano<Integers>
    {
        public override IObservable<Unit> Connect() =>
            Query
            .Update<Integers, LastIntegerAdded>(this,
                                                (integers, last) =>
                                                    last.Value = integers.Items.Any() ? integers.Items.Last().Value : int.MinValue);
    }
}
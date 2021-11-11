namespace streams.MicroServiceB.NanoServices
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using nflow.core.Flow;
    using streams.Core;
    using streams.MicroServiceA.Streams;

    public sealed class MyNanoServiceB2 : INano
    {
        public IObservable<Unit> Connect(IMicroBus bus) =>
            bus
                .Query<Bar>()
                .Select(x => Unit.Default);
    }
}

namespace streams.MicroServiceB.NanoServices
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using streams.Core;
    using streams.Core.BusComponents;
    using streams.MicroServiceA.Streams;

    public sealed class MyNanoServiceB2 : INano
    {
        public IObservable<Unit> Connect(IBus bus) =>
            bus
                .Query<Bar>()
                .Select(x => Unit.Default);
    }
}

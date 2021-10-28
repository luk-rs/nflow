namespace streams.MicroServiceB.NanoServices
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using streams.Core;
    using streams.Core.BusComponents;
    using streams.MicroServiceA.Streams;

    public sealed class MyNanoServiceB : INano
    {
        public IObservable<Unit> Connect(IBus bus) =>
            bus
                .Listen<FooEvent>()
                .Select(x => Unit.Default);
    }
}

namespace streams.MicroServiceA.NanoServices
{
    using System;
    using System.Reactive;
    using nflow.core.Flow;
    using streams.Core;
    using streams.MicroServiceA.Commands;
    using streams.MicroServiceA.Streams;

    public sealed class MyNanoServiceA : INano
    {
        public IObservable<Unit> Connect(IMicroBus bus) =>
            bus
                .Handle<FooCommand>()
                .AndPublish(bus, _ => new FooEvent(1));
    }
}


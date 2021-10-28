namespace streams.MicroServiceA.NanoServices
{
    using System;
    using System.Reactive;
    using streams.Core;
    using streams.Core.BusComponents;
    using streams.Extensions;
    using streams.MicroServiceA.Commands;
    using streams.MicroServiceA.Streams;

    public sealed class MyNanoServiceA2 : INano
    {
        public IObservable<Unit> Connect(IBus bus) =>
            bus
                .Handle<UpdateSomethingCommand>()
                .AndUpdate<Bar>(bus, (command, stream) => stream.SomeValue++);
    }
}


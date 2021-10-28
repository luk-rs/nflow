namespace streams.Core
{
    using System;
    using System.Reactive;
    using streams.Core.BusComponents;

    public interface INano
    {
        IObservable<Unit> Connect(IBus bus);
    }
}


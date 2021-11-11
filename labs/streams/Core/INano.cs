namespace streams.Core
{
    using System;
    using System.Reactive;
    using nflow.core.Flow;

    public interface INano
    {
        IObservable<Unit> Connect(IMicroBus bus);
    }
}


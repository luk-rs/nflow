namespace nflow.core
{
    using System;
    using System.Reactive;

    public interface INanoService
    {
        IObservable<Unit> Connect(IBus bus);

    }
}
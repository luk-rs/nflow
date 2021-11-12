namespace nflow.core
{
    using System;


    public interface IEventbus
    {
        IObservable<IEvent> EventsSent { get; }

        void RouteEvent(IEvent @event);
    }
}


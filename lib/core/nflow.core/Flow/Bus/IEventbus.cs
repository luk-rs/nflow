namespace nflow.core.Flow
{
    using System;


    public interface IEventbus
    {
        IObservable<IEvent> EventsSent { get; }

        void RouteEvent(IEvent @event);
    }
}


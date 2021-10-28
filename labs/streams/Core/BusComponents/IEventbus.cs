namespace streams.Core.BusComponents
{
    using System;
    using streams.Core;

    public interface IEventbus
    {
        IObservable<IEvent> EventsSent { get; }

        void RouteEvent(IEvent @event);
    }
}


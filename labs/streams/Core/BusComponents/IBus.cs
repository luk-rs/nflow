namespace streams.Core.BusComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using streams.Core;

    public interface IBus
    {
        void SendCommand(ICommand command);

        void Publish(IEvent @event);

        void Update<TStream>(Action<TStream> updateAction) where TStream : IPersistedStream, new();

        IObservable<TCommand> Handle<TCommand>() where TCommand : ICommand;

        IObservable<TPersistedStream> Query<TPersistedStream>() where TPersistedStream : IPersistedStream, new();

        IObservable<TEvent> Listen<TEvent>() where TEvent : IEvent;
    }

    public class Bus : IBus, ICommandBus, IEventbus, IStreamSource
    {
        private readonly Subject<ICommand> _commandsOut = new();
        private readonly Subject<ICommand> _commandsIn = new();

        private readonly Subject<IEvent> _eventsOut = new();
        private readonly Subject<IEvent> _eventsIn = new();

        public IObservable<ICommand> CommandsSent => _commandsOut;

        public IObservable<IEvent> EventsSent => _eventsOut;

        public void SendCommand(ICommand command) =>
            Observable
                .Return(command)
                .Do(command => _commandsOut.OnNext(command))
                .Select(_ => Unit.Default);

        public void RouteCommand(ICommand command) => _commandsIn.OnNext(command);

        public IObservable<TCommand> Handle<TCommand>() where TCommand : ICommand =>
            _commandsIn.OfType<TCommand>();

        public IObservable<TEvent> Listen<TEvent>() where TEvent : IEvent =>
            _eventsIn.OfType<TEvent>();

        public void RouteEvent(IEvent @event) => _eventsIn.OnNext(@event);

        public void Publish(IEvent @event) => _eventsOut.OnNext(@event);

        public Dictionary<Type, object> LocalStreams { get; } = new Dictionary<Type, object>();

        public Dictionary<Type, object> AllStreams { get; } = new Dictionary<Type, object>();

        public void Update<TPersistedStream>(Action<TPersistedStream> updateAction)
            where TPersistedStream : IPersistedStream, new() =>
            ((PersistedStream<TPersistedStream>)LocalStreams[typeof(TPersistedStream)])
                .Update(updateAction);

        public IObservable<TPersistedStream> Query<TPersistedStream>()
            where TPersistedStream : IPersistedStream, new() =>
            ((PersistedStream<TPersistedStream>)AllStreams[typeof(TPersistedStream)]).Stream;
    }
}

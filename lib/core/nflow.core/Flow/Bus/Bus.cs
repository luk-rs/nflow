namespace nflow.core.Flow
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    public class Bus : IMicroBus, ICommandBus, IEventbus, IStreamSource
    {
        private readonly Subject<ICommand> _commandsOut = new();
        private readonly Subject<ICommand> _commandsIn = new();

        private readonly Subject<IEvent> _eventsOut = new();
        private readonly Subject<IEvent> _eventsIn = new();

        public IObservable<ICommand> CommandsSent => _commandsOut;

        public IObservable<IEvent> EventsSent => _eventsOut;



        public void RouteCommand(ICommand command) => _commandsIn.OnNext(command);

        public IObservable<TCommand> Handle<TCommand>() where TCommand : ICommand =>
            _commandsIn.OfType<TCommand>();

        public IObservable<TEvent> Listen<TEvent>() where TEvent : IEvent =>
            _eventsIn.OfType<TEvent>();

        public void RouteEvent(IEvent @event) => _eventsIn.OnNext(@event);

        public void Gossip(IEvent @event) => _eventsOut.OnNext(@event);

        public StreamDictionary LocalStreams { get; } = new();

        public StreamDictionary AllStreams { get; } = new();


        IObservable<TPersistedStream> IMicroBus.Query<TPersistedStream>()
        {
            var oracle = ((Oracle<TPersistedStream>)AllStreams[typeof(TPersistedStream)]);
            return oracle.Stream;
        }


        public IMicroBus AddOracle<TPersistedStream>()
            where TPersistedStream : IPersistedStream, new()
        {
            LocalStreams.Add(typeof(TPersistedStream), new Oracle<TPersistedStream>());
            return this;
        }







        void IMicroBus.Update<TPersistedStream>(Action<TPersistedStream> with)
        {
            var oracle = ((Oracle<TPersistedStream>)LocalStreams[typeof(TPersistedStream)]);
            oracle.Update(with);
        }

        void IMicroBus.Send<TCommand>(Func<TCommand> command)
        {
            _commandsOut.OnNext(command());
        }

    }
}

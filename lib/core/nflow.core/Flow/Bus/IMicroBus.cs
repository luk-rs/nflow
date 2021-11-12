namespace nflow.core
{
    using System;
    using System.Reactive;

    // public interface IMicroBus
    // {
    //     //? this relates to Commands
    //     void Send<TCommand>(Func<TCommand> command) where TCommand : ICommand;
    //     IObservable<TCommand> Handle<TCommand>() where TCommand : ICommand;


    //     //? this relates to Oracles who HOLD data
    //     void Update<TPersistedStream>(Action<TPersistedStream> with) where TPersistedStream : IPersistedStream, new();
    //     IObservable<TPersistedStream> Query<TPersistedStream>() where TPersistedStream : IPersistedStream, new();


    //     //? this relates with whispers who GOSSIP data
    //     IObservable<TEvent> Listen<TEvent>() where TEvent : IEvent;
    //     void Gossip(IEvent @event);
    // }
}

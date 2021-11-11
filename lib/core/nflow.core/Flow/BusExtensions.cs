namespace nflow.core.Flow
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;

    public static class BusExtensions
    {
        public static IObservable<Unit> AndUpdate<TInput, TPersistedStream>(
            this IObservable<TInput> input,
            IMicroBus bus,
            Func<TPersistedStream, TInput, TPersistedStream> with)
            where TPersistedStream : IPersistedStream, new() =>
            input
                .Do(input => bus.Update<TPersistedStream>(stream => with(stream, input)))
                .Select(_ => Unit.Default);

        public static IObservable<Unit> AndUpdate<TPersistedStream>(
            this IObservable<ICommand> command,
            IMicroBus bus,
            Action<ICommand, TPersistedStream> updateWith)
            where TPersistedStream : IPersistedStream, new() =>
            command
                .Do(command => bus.Update<TPersistedStream>(stream => updateWith(command, stream)))
                .Select(_ => Unit.Default);

        public static IObservable<Unit> AndSend<TInput, TCommand>(
           this IObservable<TInput> input,
           IMicroBus bus,
           Func<TCommand> command)
            where TCommand : ICommand =>
           input
               .Do(_ => bus.Send(command))
               .Select(_ => Unit.Default);

        public static IObservable<Unit> AndPublish<TEvent>(
           this IObservable<ICommand> command,
           IMicroBus bus,
           Func<ICommand, TEvent> @event)
           where TEvent : IEvent =>
               command
                   .Do(command => bus.Gossip(@event(command)))
                   .Select(_ => Unit.Default);
    }
}


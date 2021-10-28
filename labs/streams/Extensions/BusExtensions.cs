namespace streams.Extensions
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using streams.Core;
    using streams.Core.BusComponents;

    public static class BusExtensions
    {
        public static IObservable<Unit> AndUpdate<TInput, TPersistedStream>(
            this IObservable<TInput> input,
            IBus bus,
            Action<TInput, TPersistedStream> updateAction)
            where TPersistedStream : IPersistedStream, new() =>
            input
                .Do(input => bus.Update<TPersistedStream>(stream => updateAction(input, stream)))
                .Select(_ => Unit.Default);

        public static IObservable<Unit> AndUpdate<TPersistedStream>(
            this IObservable<ICommand> command,
            IBus bus,
            Action<ICommand, TPersistedStream> updateAction)
            where TPersistedStream : IPersistedStream, new() =>
            command
                .Do(command => bus.Update<TPersistedStream>(stream => updateAction(command, stream)))
                .Select(_ => Unit.Default);

        public static IObservable<Unit> AndSend<TInput>(
           this IObservable<TInput> input,
           IBus bus,
           ICommand command) =>
           input
               .Do(_ => bus.SendCommand(command))
               .Select(_ => Unit.Default);

        public static IObservable<Unit> AndPublish<TEvent>(
           this IObservable<ICommand> command,
           IBus bus,
           Func<ICommand, TEvent> @event)
           where TEvent : IEvent =>
               command
                   .Do(command => bus.Publish(@event(command)))
                   .Select(_ => Unit.Default);
    }
}


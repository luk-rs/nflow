namespace Flow.Reactive.Extensions
{

    using System;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Runtime.CompilerServices;
    using Streams.Ephemeral.Commands;


    public static class CommandExtensions
    {
        public static IObservable<Unit> SendCommand<TPayload, TCommand>(this IFlow flow,
                                                                        IObservable<TPayload> payload,
                                                                        Func<TPayload, TCommand> commandSelector,
                                                                        [CallerFilePath] string sender = "")
                                                                        where TCommand : Command
            => payload.SendCommand(flow, commandSelector, sender);

        public static IObservable<Unit> SendCommand<TPayload, TCommand>(this IFlow flow,
                                                                       IObservable<TPayload> payload,
                                                                       Func<TPayload, TCommand> commandSelector,
                                                                       IScheduler scheduler,
                                                                       [CallerFilePath] string sender = "")
                                                                       where TCommand : Command
            => payload.SendCommand(flow, commandSelector, scheduler, sender);

        public static IObservable<Unit> SendCommand<TPayload, TCommand>(this IObservable<TPayload> payload,
                                                                        IFlow flow,
                                                                        Func<TPayload, TCommand> commandSelector,
                                                                        [CallerFilePath] string sender = "")
                                                                        where TCommand : Command
            => payload.SendCommand(flow, commandSelector, Scheduler.Default, sender);

        public static IObservable<Unit> SendCommand<TPayload, TCommand>(this IObservable<TPayload> payload,
                                                                        IFlow flow,
                                                                        Func<TPayload, TCommand> commandSelector,
                                                                        IScheduler scheduler,
                                                                        [CallerFilePath] string sender = "")
                                                                        where TCommand : Command
           => payload
                .ObserveOn(scheduler)
                .Select(commandSelector)
                .Do(command => command.Trace = true)
                .Select(command => flow.Send(command, sender))
                .Switch();
    }
}

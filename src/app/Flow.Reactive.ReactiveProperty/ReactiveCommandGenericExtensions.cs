namespace Flow.Reactive.ReactiveProperty
{
    using System;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using Extensions;
    using global::Reactive.Bindings;
    using Rx.Extensions;
    using Streams.Ephemeral.Commands;

    public static class ReactiveCommandGenericExtensions
    {
        public static ReactiveCommand<T> BindCommand<T, TCommand>(this IFlow flow,
                                                                  Func<T, TCommand> command,
                                                                  CompositeDisposable disposables)
        where TCommand : Command
            => flow.BindCommand(command, Observable.Return(true), Scheduler.Default, disposables);

        public static ReactiveCommand<T> BindCommand<T, TCommand>(this IFlow flow,
                                                                  Func<T, TCommand> command,
                                                                  IObservable<bool> canExecute,
                                                                  CompositeDisposable disposables)
        where TCommand : Command
            => flow.BindCommand(command, canExecute, Scheduler.Default, disposables);

        public static ReactiveCommand<T> BindCommand<T, TCommand>(this IFlow flow,
                                                                  Func<T, TCommand> command,
                                                                  IScheduler scheduler,
                                                                  CompositeDisposable disposables)
        where TCommand : Command
            => flow.BindCommand(command, Observable.Return(true), scheduler, disposables);

        public static ReactiveCommand<T> BindCommand<T, TCommand>(this IFlow flow,
                                                                  Func<T, TCommand> command,
                                                                  IObservable<bool> canExecute,
                                                                  IScheduler scheduler,
                                                                  CompositeDisposable disposables)
       where TCommand : Command
        {
            var reactiveCommand = canExecute
                .ToReactiveCommand<T>()
                .AddToDisposables(disposables);

            reactiveCommand
               .SendCommand(flow, command, scheduler)
               .Subscribe()
               .AddToDisposables(disposables);

            return reactiveCommand;
        }
    }
}

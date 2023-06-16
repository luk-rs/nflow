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
    using Streams.Persisted;

    public static class ReactiveCommandExtensions
    {
        public static ReactiveCommand BindCommand<TCommand, TStreamData>(this IFlow flow,
                                                                         Func<TStreamData, TCommand> command,
                                                                         IObservable<bool> canExecute,
                                                                         IScheduler scheduler,
                                                                         CompositeDisposable disposables)
            where TCommand : Command
            where TStreamData : PersistedStreamData
        {
            var reactiveCommand = canExecute
                .ToReactiveCommand()
                .AddToDisposables(disposables);

            reactiveCommand
                .WithLatestFrom(flow.Query<TStreamData>(), (_, query) => query)
                .SendCommand(flow, command, scheduler)
                .Subscribe()
                .AddToDisposables(disposables);

            return reactiveCommand;
        }

        public static ReactiveCommand BindCommand<TCommand, TCanExecuteStreamData, TStreamData>(this IFlow flow,
                                                                                                Func<TStreamData, TCommand> command,
                                                                                                Func<IObservable<TCanExecuteStreamData>, IObservable<bool>> canExecute,
                                                                                                IScheduler scheduler,
                                                                                                CompositeDisposable disposables)
           where TCommand : Command
           where TCanExecuteStreamData : PersistedStreamData
           where TStreamData : PersistedStreamData
        {
            var reactiveCommand =
                canExecute(flow.Query<TCanExecuteStreamData>())
                .ToReactiveCommand()
                .AddToDisposables(disposables);

            reactiveCommand
                .WithLatestFrom(flow.Query<TStreamData>(), (_, query) => query)
                .SendCommand(flow, command, scheduler)
                .Subscribe()
                .AddToDisposables(disposables);

            return reactiveCommand;
        }

        public static ReactiveCommand BindCommand<TCommand, TStreamData>(this IFlow flow,
                                                                         Func<TStreamData, TCommand> command,
                                                                         Func<IObservable<TStreamData>, IObservable<bool>> canExecute,
                                                                         IScheduler scheduler,
                                                                         CompositeDisposable disposables)
           where TCommand : Command
           where TStreamData : PersistedStreamData
        {
            var reactiveCommand = 
                canExecute(flow.Query<TStreamData>())
                .ToReactiveCommand()
                .AddToDisposables(disposables);

            reactiveCommand
                .WithLatestFrom(flow.Query<TStreamData>(), (_, query) => query)
                .SendCommand(flow, command, scheduler)
                .Subscribe()
                .AddToDisposables(disposables);

            return reactiveCommand;
        }

        public static ReactiveCommand BindCommand<TCommand>(this IFlow flow,
                                                            Func<object, TCommand> command,
                                                            IObservable<bool> canExecute,
                                                            IScheduler scheduler,
                                                            CompositeDisposable disposables)
            where TCommand : Command
        {
            var reactiveCommand = canExecute
                .ToReactiveCommand()
                .AddToDisposables(disposables);

            reactiveCommand
                .SendCommand(flow, command, scheduler)
                .Subscribe()
                .AddToDisposables(disposables);

            return reactiveCommand;
        }

        public static ReactiveCommand BindCommand<TCommand>(this IFlow flow,
                                                            Func<object, TCommand> command,
                                                            CompositeDisposable disposables)
            where TCommand : Command =>
            flow.BindCommand(command, Scheduler.Default, disposables);

        public static ReactiveCommand BindCommand<TCommand>(this IFlow flow,
                                                            Func<object, TCommand> command,
                                                            IObservable<bool> canExecute,
                                                            CompositeDisposable disposables)
            where TCommand : Command =>
            flow.BindCommand(command, canExecute, Scheduler.Default, disposables);

        public static ReactiveCommand BindCommand<TCommand, TCanExecute>(this IFlow flow,
                                                                         Func<object, TCommand> command,
                                                                         Predicate<TCanExecute> canExecute,
                                                                         IScheduler scheduler,
                                                                         CompositeDisposable disposables)
          where TCommand : Command
          where TCanExecute : PersistedStreamData =>
          flow.BindCommand(command,
                           flow
                               .Query<TCanExecute>()
                               .Select(x => canExecute(x)),
                           scheduler,
                           disposables);

        public static ReactiveCommand BindCommand<TCommand, TCanExecute>(this IFlow flow,
                                                                        Func<object, TCommand> command,
                                                                        Predicate<TCanExecute> canExecute,
                                                                        CompositeDisposable disposables)
         where TCommand : Command
         where TCanExecute : PersistedStreamData =>
         flow.BindCommand(command, canExecute, Scheduler.Default, disposables);

        public static ReactiveCommand BindCommand<TCommand>(this IFlow flow,
                                                            TCommand command,
                                                            IObservable<bool> canExecute,
                                                            IScheduler scheduler,
                                                            CompositeDisposable disposables)
            where TCommand : Command =>
           BindCommand(flow, _ => command, canExecute, scheduler, disposables);

        public static ReactiveCommand BindCommand<TCommand>(this IFlow flow,
                                                            TCommand command,
                                                            IObservable<bool> canExecute,
                                                            CompositeDisposable disposables)
             where TCommand : Command =>
            BindCommand(flow, command, canExecute, Scheduler.Default, disposables);

        public static ReactiveCommand BindCommand<TCommand, TCanExecute>(this IFlow flow,
                                                                         TCommand command,
                                                                         Predicate<TCanExecute> canExecute,
                                                                         IScheduler scheduler,
                                                                         CompositeDisposable disposables)
            where TCommand : Command
            where TCanExecute : PersistedStreamData =>
            BindCommand(flow, _ => command, canExecute, scheduler, disposables);

        public static ReactiveCommand BindCommand<TCommand, TCanExecute>(this IFlow flow,
                                                                         TCommand command,
                                                                         Predicate<TCanExecute> canExecute,
                                                                         CompositeDisposable disposables)
          where TCommand : Command
          where TCanExecute : PersistedStreamData =>
          BindCommand(flow, command, canExecute, Scheduler.Default, disposables);


        public static ReactiveCommand BindCommand<TCommand>(this IFlow flow,
                                                            TCommand command,
                                                            IScheduler scheduler,
                                                            CompositeDisposable disposables)
           where TCommand : Command =>
               BindCommand(flow, command, Observable.Return(true), scheduler, disposables);

        public static ReactiveCommand BindCommand<TCommand>(this IFlow flow,
                                                            TCommand command,
                                                            CompositeDisposable disposables)
            where TCommand : Command =>
                BindCommand(flow, command, Scheduler.Default, disposables);

        public static ReactiveCommand BindCommand<TCommand>(this IFlow flow,
                                                            Func<object, TCommand> command,
                                                            IScheduler scheduler,
                                                            CompositeDisposable disposables)
           where TCommand : Command =>
               BindCommand(flow, command, Observable.Return(true), scheduler, disposables);

        public static ReactiveCommand BindCommand<TCommand, TStreamData>(this IFlow flow,
                                                                         Func<TStreamData, TCommand> command,
                                                                         CompositeDisposable disposables)
            where TCommand : Command
            where TStreamData : PersistedStreamData =>
            BindCommand(flow, command, Observable.Return(true), Scheduler.Default, disposables);

        public static ReactiveCommand BindCommand<TCommand, TStreamData>(this IFlow flow,
                                                                         Func<TStreamData, TCommand> command,
                                                                         IObservable<bool> canExecute,
                                                                         CompositeDisposable disposables)
            where TCommand : Command
            where TStreamData : PersistedStreamData =>
            BindCommand(flow, command, canExecute, Scheduler.Default, disposables);

        public static ReactiveCommand BindCommand<TCommand, TStreamData>(this IFlow flow,
                                                                         Func<TStreamData, TCommand> command,
                                                                         IScheduler scheduler,
                                                                         CompositeDisposable disposables)
           where TCommand : Command
           where TStreamData : PersistedStreamData =>
           BindCommand(flow, command, Observable.Return(true), scheduler, disposables);

        public static ReactiveCommand BindCommand<TCommand, TStreamData>(this IFlow flow,
                                                                         Func<TStreamData, TCommand> command,
                                                                         Func<IObservable<TStreamData>, IObservable<bool>> canExecute,
                                                                         CompositeDisposable disposables)
            where TCommand : Command
            where TStreamData : PersistedStreamData =>
            BindCommand< TCommand,TStreamData>(flow, command, canExecute, Scheduler.Default, disposables);

        public static ReactiveCommand BindCommand<TCommand, TCanExecuteStreamData, TStreamData>(this IFlow flow,
                                                                                               Func<TStreamData, TCommand> command,
                                                                                               Func<IObservable<TCanExecuteStreamData>, IObservable<bool>> canExecute,
                                                                                               CompositeDisposable disposables)
          where TCommand : Command
          where TCanExecuteStreamData : PersistedStreamData
          where TStreamData : PersistedStreamData =>
            BindCommand(flow, command, canExecute, Scheduler.Default, disposables);

        public static ReactiveCommand BindCommandWithValidation<TCommand>(this IFlow flow,
                                                                  Func<object, TCommand> command,
                                                                  Func<bool> hasErrors,
                                                                  CompositeDisposable disposables)
            where TCommand : Command
        {
            var reactiveCommand = new ReactiveCommand()
                .AddToDisposables(disposables);

            reactiveCommand
              .Where(_ => !hasErrors())
              .SendCommand(flow, command, Scheduler.Default)
              .Subscribe()
              .AddToDisposables(disposables);

            return reactiveCommand;
        }
    }
}

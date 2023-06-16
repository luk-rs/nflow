namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Concurrency;
    using Streams.Ephemeral.Commands;


    public abstract class CommandToActionNano<TCommand> : Nano
      where TCommand : Command
    {
        public override int StartOrder => 500;

        public override IObservable<Unit> Connect()
            => Handle<TCommand>()
                .ObserveOn(GetScheduler())
                .Do(command => Action(command))
                .Select(_ => Unit.Default);

        protected abstract Action<TCommand> Action { get; }

        protected virtual IScheduler GetScheduler() => Scheduler.Default;
    }
}
namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Streams.Ephemeral.Commands;
    using Streams.Persisted;


    public abstract class CommandToQueryStreamNano<TCommand, TStreamData> : Nano
        where TCommand : Command
        where TStreamData : PersistedStreamData
    {
        public override int StartOrder => 500;

        public override IObservable<Unit> Connect()
            => Handle<TCommand>()
                .Select(command => Update<TStreamData>(x => Updater(command)(x)))
                .Concat();

        protected abstract Func<TCommand, Action<TStreamData>> Updater { get; }
    }
}
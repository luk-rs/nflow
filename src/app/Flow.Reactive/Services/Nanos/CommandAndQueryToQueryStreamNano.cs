namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Services;
    using Streams.Ephemeral.Commands;
    using Streams.Persisted;


    public abstract class CommandAndQueryToQueryStreamNano<TCommand, TQueryReadStreamData, TQueryWriteStreamData> : Nano
            where TCommand : Command
            where TQueryReadStreamData : PersistedStreamData
            where TQueryWriteStreamData : PersistedStreamData
    {

        public override int StartOrder => 500;

        public override IObservable<Unit> Connect() => Handle<TCommand>()
                                                      .WithLatestFrom(Query<TQueryReadStreamData>(), (command, queryResult) => (command, queryResult))
                                                      .Select(commandAndQuery => Update<TQueryWriteStreamData>(x => Updater(commandAndQuery)(x)))
                                                      .Concat();

        protected abstract Func<(TCommand command, TQueryReadStreamData queryResult), Action<TQueryWriteStreamData>> Updater { get; }

    }

}
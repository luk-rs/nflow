namespace Flow.Reactive.Services.Nanos
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Services;
    using Streams.Ephemeral;
    using Streams.Ephemeral.Commands;
    using Streams.Persisted;


    public abstract class CommandAndQueryToEventStreamNano<TCommand, TQueryStreamData, TEventStreamData> : Nano
        where TCommand : Command
        where TQueryStreamData : PersistedStreamData
        where TEventStreamData : StreamData
    {
        public override int StartOrder => 500;

        public override IObservable<Unit> Connect()
           => Handle<TCommand>()
                .WithLatestFrom(Query<TQueryStreamData>(), (command, queryResult) => (command, queryResult))
                .Select(x => Notify(CreateEvent(x)))
                .Concat();

        protected abstract Func<(TCommand command, TQueryStreamData queryResult), TEventStreamData> CreateEvent { get; }
    }
}

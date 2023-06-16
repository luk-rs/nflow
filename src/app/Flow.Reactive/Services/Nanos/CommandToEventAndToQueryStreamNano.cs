namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Services;
    using Streams.Ephemeral;
    using Streams.Ephemeral.Commands;
    using Streams.Persisted;


    public abstract class CommandToEventAndToQueryStreamNano<TCommand, TEventStreamData, TQueryStreamData> : Nano
            where TCommand : Command
            where TEventStreamData : StreamData
            where TQueryStreamData : PersistedStreamData
    {

        public override int StartOrder => 500;

        public override IObservable<Unit> Connect() => Handle<TCommand>()
                                                      .Select(command => Notify(CreateEvent(command)).Merge(Update<TQueryStreamData>(x => Updater(command)(x))))
                                                      .Concat();

        protected abstract Func<TCommand, TEventStreamData> CreateEvent { get; }

        protected abstract Func<TCommand, Action<TQueryStreamData>> Updater { get; }

    }

}
namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Streams.Ephemeral;
    using Streams.Ephemeral.Commands;


    public abstract class CommandToEventStreamNano<TCommand, TStreamData> : Nano
        where TCommand : Command
        where TStreamData : StreamData
    {
        public override int StartOrder => 500;

        public override IObservable<Unit> Connect()
            => Handle<TCommand>()
                .Select(command => Notify(CreateEvent(command)))
                .Concat();

        protected abstract Func<TCommand, TStreamData> CreateEvent { get; }
    }
}
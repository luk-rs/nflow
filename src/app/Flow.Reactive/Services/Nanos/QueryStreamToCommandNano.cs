namespace Flow.Reactive.Services.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Streams.Ephemeral.Commands;
    using Streams.Persisted;


    public abstract class QueryStreamToCommandNano<TStreamData, TCommand> : Nano
      where TStreamData : PersistedStreamData
      where TCommand : Command
    {
        public override int StartOrder => 2000;

        public override IObservable<Unit> Connect()
            => Query<TStreamData>()
                .Where(queryData => Filter(queryData))
                .Select(queryData => Send(Command(queryData)))
                .Concat();

        protected abstract Func<TStreamData, TCommand> Command { get; }

        protected virtual Predicate<TStreamData> Filter { get; } = _ => true;
    }
}
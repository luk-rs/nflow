namespace Flow.Reactive.Services
{
    using Flow.Rx.Extensions;
    using Streams.Ephemeral;
    using Streams.Ephemeral.Commands;
    using Streams.Persisted;
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reflection;

    public interface INano
    {

        internal IMicro Micro { get; set; }
        int StartOrder { get; }
        IObservable<Unit> Connect();

    }


    public abstract class Nano : INano
    {

        IMicro INano.Micro { get; set; }

        public virtual int StartOrder { get; } = 1000;

        public abstract IObservable<Unit> Connect();

        /// <summary>
        /// Handle any given command on the current microservice
        /// </summary>
        /// <typeparam name="TCommand">the type of the command being handled</typeparam>
        /// <returns>the observable with reference to the ordered command request</returns>
        internal IObservable<TCommand> Handle<TCommand>() where TCommand : Command => ((INano)this).Micro.Handle<TCommand>();

        /// <summary>
        /// Notify event streams of new data
        /// </summary>
        /// <typeparam name="TStreamData">the type of newly generated stream data</typeparam>
        /// <param name="event">the data payload that will be notified</param>
        /// <returns>the observable containing the promise of the execution</returns>
        internal IObservable<Unit> Notify<TStreamData>(TStreamData @event)
                where TStreamData : StreamData => ((INano)this).Micro.Notify(@event);

        internal IObservable<Unit> Send<TCommand>(TCommand command) where TCommand : Command => ((INano)this).Micro.Send(command);

        /// <summary>
        /// Generate new data on queryable streams
        /// </summary>
        /// <typeparam name="TStreamData">the type of newly generated stream data</typeparam>
        /// <param name="nano">the source nano that's generating data</param>
        /// <param name="update">the update action of the current stream data</param>
        /// <returns>the observable containing the promise of the execution</returns>
        internal IObservable<Unit> Update<TStreamData>(Action<TStreamData> update)
                where TStreamData : PersistedStreamData => ((INano)this).Micro.Update(update);

        internal IObservable<TStreamData> Query<TStreamData>()
                where TStreamData : PersistedStreamData => ((INano)this).Micro.Query<TStreamData>();

        internal IObservable<TStreamData> Listen<TStreamData>()
                where TStreamData : StreamData => ((INano)this).Micro.Listen<TStreamData>();

    }


    public abstract class TriggerNano<T> : Nano
    {

        public sealed override int StartOrder => Index * 100 + Order;

        protected virtual int Index => 50;
        protected virtual int Order => 0;
        protected abstract IObservable<T> Trigger { get; }

        protected IObservable<(T Trigger, TStreamData Query)> TriggerWithQuery<TStreamData>()
                where TStreamData : PersistedStreamData => Trigger.WithLatestFrom(Query<TStreamData>(), (trigger, query) => (trigger, query));

        protected IObservable<(T Trigger, TStreamData Query)> TriggerAndCombineQuery<TStreamData>()
                where TStreamData : PersistedStreamData => Trigger.CombineLatest(Query<TStreamData>(), (trigger, query) => (trigger, query));

    }


    public abstract class HandlerNano<TCommand> : TriggerNano<TCommand>
            where TCommand : Command
    {

        protected sealed override int Index => 5;
        protected sealed override IObservable<TCommand> Trigger => Handle<TCommand>();

        protected IObservable<TCommand> Handle => Trigger;

        protected IObservable<(TCommand Command, TStreamData Query)> HandleAndQuery<TStreamData>()
                where TStreamData : PersistedStreamData => TriggerWithQuery<TStreamData>();

        protected IObservable<(TCommand Command, TStreamData1 Query1, TStreamData2 Query2)> HandleAndQuery<TStreamData1, TStreamData2>()
               where TStreamData1 : PersistedStreamData
               where TStreamData2 : PersistedStreamData =>
                TriggerWithQuery<TStreamData1>()
                .WithLatestFrom(Query<TStreamData2>(), (x, Query2) => (x.Trigger, x.Query, Query2));

    }


    public abstract class QueryNano<TStreamData> : TriggerNano<TStreamData>
            where TStreamData : PersistedStreamData
    {

        protected sealed override int Index => 20;
        protected sealed override IObservable<TStreamData> Trigger => Query<TStreamData>();

        protected IObservable<TStreamData> Query => Query<TStreamData>();

        protected IObservable<(TStreamData FirstQuery, TOtherStreamData SecondQuery)> QueryWithQuery<TOtherStreamData>()
                where TOtherStreamData : PersistedStreamData => TriggerWithQuery<TOtherStreamData>();

        protected IObservable<(TStreamData FirstQuery, TOtherStreamData SecondQuery)> QueryAndCombineQuery<TOtherStreamData>()
                where TOtherStreamData : PersistedStreamData => TriggerAndCombineQuery<TOtherStreamData>();
    }

    public abstract class EventListenerNano<TStreamData> : TriggerNano<TStreamData>
            where TStreamData : StreamData
    {

        protected sealed override int Index => 5;

        protected sealed override IObservable<TStreamData> Trigger => Listen<TStreamData>();

        protected IObservable<TStreamData> Listen => Listen<TStreamData>();

        protected IObservable<(TStreamData Event, TQueryStreamData Query)> ListenAndQuery<TQueryStreamData>()
             where TQueryStreamData : PersistedStreamData => TriggerWithQuery<TQueryStreamData>();

        protected IObservable<(TStreamData Event, TQueryStreamData Query)> ListenAndCombineQuery<TQueryStreamData>()
            where TQueryStreamData : PersistedStreamData => Listen.StartWith(default(TStreamData)).CombineLatest(Query<TQueryStreamData>(), (l, q) => (l, q));
    }

    public static class NanoExtensions
    {

        public static IObservable<Unit> PerformAction<TInput>(this IObservable<TInput> value, Action<TInput> action)
            => value.Do(action)
                    .Select(_ => Unit.Default);

        public static IObservable<TInput> PerformActionAnd<TInput>(this IObservable<TInput> value, Action<TInput> action) =>
            value.Do(action);

        public static IObservable<Unit> Update<TInput, TStreamData>(this IObservable<TInput> value, INano nano, Action<TInput, TStreamData> updater)
                where TStreamData : PersistedStreamData => value.Select(x => ((Nano)nano).Update<TStreamData>(data => updater(x, data)))
                                                                .Merge();

        public static IObservable<Unit> Update<TInput>(this IObservable<TInput> value, INano nano, Func<TInput, Type> streamDataType, Func<TInput, Action<dynamic>> content) =>
            value
                .Select(x =>
                {
                    var method = typeof(Nano).GetMethod(nameof(Nano.Update), BindingFlags.Instance | BindingFlags.NonPublic);
                    var generic = method.MakeGenericMethod(streamDataType(x));
                    return (IObservable<Unit>)generic.Invoke(nano, new[] { content(x) });
                }).Merge();

        public static IObservable<TInput> UpdateAnd<TInput, TStreamData>(this IObservable<TInput> value, INano nano, Action<TInput, TStreamData> updater)
                where TStreamData : PersistedStreamData => value.Select(x => ((Nano)nano).Update<TStreamData>(data => updater(x, data)).Select(_ => x))
                                                                .Merge();

        public static IObservable<Unit> UpdateTwo<TInput, TStreamData1, TStreamData2>(this IObservable<TInput> value, INano nano, Action<TInput, TStreamData1> updater1, Action<TInput, TStreamData2> updater2)
              where TStreamData1 : PersistedStreamData
              where TStreamData2 : PersistedStreamData
        {
            var update1Obs = value.Select(x => ((Nano)nano).Update<TStreamData1>(data => updater1(x, data)));
            var update2Obs = value.Select(x => ((Nano)nano).Update<TStreamData2>(data => updater2(x, data)));

            return Observable.Merge(update1Obs.Merge(), update2Obs.Merge());
        }

        public static IObservable<Unit> Notify<TInput, TStreamData>(this IObservable<TInput> value, INano nano, Func<TInput, TStreamData> @event)
                where TStreamData : StreamData => value.Select(x => ((Nano)nano).Notify(@event(x)))
                                                       .Merge();

        public static IObservable<Unit> Notify<TInput>(this IObservable<TInput> value, INano nano, Func<TInput, Type> notificationType, Func<TInput, object> content) =>
            value
                .Select(x =>
                {
                    var method = typeof(Nano).GetMethod(nameof(Nano.Notify), BindingFlags.Instance | BindingFlags.NonPublic);
                    var generic = method.MakeGenericMethod(notificationType(x));
                    return (IObservable<Unit>)generic.Invoke(nano, new[] { content(x) });
                }).Merge();

        public static IObservable<TInput> NotifyAnd<TInput, TStreamData>(this IObservable<TInput> value, INano nano, Func<TInput, TStreamData> @event)
                where TStreamData : StreamData => value.Select(x => ((Nano)nano).Notify(@event(x)).Select(_ => x))
                                                       .Merge();

        public static IObservable<Unit> SendCommand<TInput, TCommand>(this IObservable<TInput> value, INano nano, Func<TInput, TCommand> selector)
                where TCommand : Command => value.Select(data => ((Nano)nano).Send(selector(data)))
                                                 .Merge();

        public static IObservable<TInput> SendCommandAnd<TInput, TCommand>(this IObservable<TInput> value, INano nano, Func<TInput, TCommand> selector)
               where TCommand : Command => value.Select(data => ((Nano)nano).Send(selector(data)).Select(_ => data))
                                                .Merge();

        public static IObservable<(TInput Data, TQuery Query)> AndQuery<TInput, TQuery>(this IObservable<TInput> value, INano nano)
              where TQuery : PersistedStreamData => value.WithLatestFrom(((Nano)nano).Query<TQuery>(), (Data, Query) => (Data, Query));

        public static IObservable<(TInput Data, TQuery Query)> AndCombineQuery<TInput, TQuery>(this IObservable<TInput> value, INano nano)
              where TQuery : PersistedStreamData => value.CombineLatest(((Nano)nano).Query<TQuery>(), (Data, Query) => (Data, Query));

        public static IObservable<TInput> AndListen<TInput, TStreamData>(this IObservable<TInput> value, INano nano)
            where TStreamData : StreamData =>
                value
                    .SwitchSelect(input =>
                        ((Nano)nano)
                            .Listen<TStreamData>()
                            .Select(_ => input)
                            .StartWith(input));

        public static IObservable<TInput> AndListenWhen<TInput, TStreamData>(this IObservable<TInput> value, INano nano, Func<TInput, TStreamData, bool> predicate)
            where TStreamData : StreamData =>
                value
                    .SwitchSelect(input =>
                        ((Nano)nano)
                            .Listen<TStreamData>()
                            .Where(@event => predicate(input, @event))
                            .Select(_ => input)
                            .StartWith(input));

        public static IObservable<TOutput> Fork<TInput, TOutput>(this IObservable<TInput> source, params (Predicate<TInput> condition, Func<TInput, IObservable<TOutput>> selector)[] forks)
            => source
              .Select(source => forks
                               .Where(fork => fork.condition(source))
                               .Select(fork => fork.selector(source)))
              .SelectMany(forks => forks)
              .Merge();

        public static IObservable<T> Trace<T>(this IObservable<T> source, Func<T, string> message, string traceRoot = "Flow.Reactive") =>
            source
            .Do(t => System.Diagnostics.Trace.WriteLine($"{traceRoot} : {message(t)}"));
    }
}
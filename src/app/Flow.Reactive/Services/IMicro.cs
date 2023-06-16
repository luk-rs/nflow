namespace Flow.Reactive.Services
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using Extensions;
    using Flow.Reactive.CustomExceptions;
    using Flow.Rx.Extensions;
    using Streams;
    using Streams.Ephemeral;
    using Streams.Ephemeral.Commands;
    using Streams.Middleware;
    using Streams.Persisted;


    internal interface IMicro
    {

        IObservable<Unit> Send<TCommand>(TCommand command)
                where TCommand : Command;

        IObservable<TCommand> Handle<TCommand>() where TCommand : Command;
        
        [Obsolete("This method has been deprecated in favor of Query, Handle and Listen typed methods")]
        IObservable<TStreamData> ReadStreamData<TStreamData>() where TStreamData : IStreamData;

        IObservable<Unit> Notify<TStreamData>(TStreamData @event) where TStreamData : StreamData;

        IObservable<Unit> Update<TStreamData>(Action<TStreamData> update) where TStreamData : PersistedStreamData;

        IObservable<TStreamData> Query<TStreamData>() where TStreamData : PersistedStreamData;

        IObservable<TStreamData> Listen<TStreamData>() where TStreamData : StreamData;
    }


    internal interface IMicroFlow : IDisposable
    {

        string Id { get; set; }

        IEnumerable<IStream> PublicStreams { get; set; }

        IEnumerable<INano> Nanos { get; set; }

        void ResetPersistedStreams();

        //TODO - This property is the current way for MasterFlow to access Middlewares, remove it after the ServiceProvider refactor
        IEnumerable<IMiddleware> Middlewares { get; }
    }


    internal sealed class Micro : IMicroFlow, IMicro
    {

        public IObservable<Unit> Send<TCommand>(TCommand command)
                where TCommand : Command =>
            Observable
                .Return(command)
                .Do(command => Middlewares.ToList().ForEach(mw => mw.Intercept(command)))
                .ConcatSelect(command => Flow.Send(command));

        public IObservable<TCommand> Handle<TCommand>() where TCommand : Command => Flow.Handle<TCommand>();
        
        [Obsolete("This method has been deprecated in favor of Query, Handle and Listen typed methods")]
        public IObservable<TStreamData> ReadStreamData<TStreamData>() where TStreamData : IStreamData => Streams
                                                                                                        .Where(stream => stream is IStream<TStreamData>)
                                                                                                        .Cast<IStream<TStreamData>>()
                                                                                                        .Select(stream => stream.Data)
                                                                                                        .Single();

        public IObservable<Unit> Notify<TStreamData>(TStreamData @event) where TStreamData : StreamData => Observable.Return(Stream<EventsStream<TStreamData>, TStreamData>())
                                                                                                                     .Select(stream => stream.Notify(@event))
                                                                                                                     .Do(data => Middlewares.ToList().ForEach(mw => mw.Intercept(data)))
                                                                                                                     .Select(_ => Unit.Default);

        public IObservable<Unit> Update<TStreamData>(Action<TStreamData> update) where TStreamData : PersistedStreamData => Observable.Return(Stream<PersistedStream<TStreamData>, TStreamData>())
                                                                                                                                   .Select(stream => stream.Update(update))
                                                                                                                                   .Do(data => Middlewares.ToList().ForEach(mw => mw.Intercept(data)))
                                                                                                                                   .Select(_ => Unit.Default);

        public IObservable<TStreamData> Query<TStreamData>() where TStreamData : PersistedStreamData => Observable.Return(Stream<PersistedStream<TStreamData>, TStreamData>())
                                                                                                                  .SwitchSelect(stream => stream.Data);

        public IObservable<TStreamData> Listen<TStreamData>() where TStreamData : StreamData => Observable.Return(Stream<EventsStream<TStreamData>, TStreamData>())
                                                                                                          .SwitchSelect(stream => stream.Data);

        public void ResetPersistedStreams() => 
            ((IMicroFlow)this)
                .PublicStreams
                .Concat(InternalStreams)
                .OfType<IPersistedStream>()
                .ToList()
                .ForEach(stream => stream.Reset());

        string IMicroFlow.Id { get; set; }

        IEnumerable<IStream> IMicroFlow.PublicStreams { get; set; }

        IEnumerable<INano> IMicroFlow.Nanos { get; set; }

        #region construction

        public Micro(string id, IEnumerable<IStream> allStreams, IEnumerable<INano> nanoServices, IEnumerable<IMiddleware> middlewares)
            : this(id, allStreams, nanoServices, middlewares, transient: false)
        {
        }

        public Micro(string id, IEnumerable<IStream> allStreams, IEnumerable<INano> nanoServices, IEnumerable<IMiddleware> middlewares, bool transient)
        {
            ((IMicroFlow)this).Id = id;

            (IEnumerable<IStream> publics, IEnumerable<IStream> internals) = 
                allStreams.Aggregate(
                    (publics: new IStream[0].AsEnumerable(), internals: new IStream[0].AsEnumerable()),
                    (acc, cur) =>
                    {
                        var stream = new[]
                        {
                                cur
                        };
                        var update = (cur.Public ? acc.publics : acc.internals).Concat(stream);

                        return cur.Public ? (update, acc.internals) : (acc.publics, update);
                    });

            ((IMicroFlow)this).PublicStreams = publics;
            InternalStreams = internals;

            ((IMicroFlow)this).Nanos = nanoServices
                   .Do(nano => nano.Micro = this)
                   .Do(nano => Trace.WriteLine($"Flow Reactive Setup : Micro {((IMicroFlow)nano.Micro).Id} Nano {nano.GetType().Name}"))
                   .OrderBy(nano => nano.StartOrder)
                   .ToList();

            publics
                .ToList()
                .ForEach(stream => Trace.WriteLine($"Flow Reactive Setup : Public Stream - {stream.GetType().Name}"));

            Middlewares = middlewares;
            LifeTimeScope = transient 
                ? new LifeTimeScope.TransientStopped() 
                : new LifeTimeScope.Singleton();
        }

        public void Dispose() => Streams.ToList().ForEach(stream => stream.Dispose());

        #endregion

        internal LifeTimeScope LifeTimeScope { get; set; }
        internal IFlowMicro Flow { get; set; }
        private IEnumerable<IMiddleware> Middlewares { get; }

        IEnumerable<IMiddleware> IMicroFlow.Middlewares => Middlewares;

        private IEnumerable<IStream> Streams => InternalStreams.Concat(Flow.Streams);
        private IEnumerable<IStream> InternalStreams { get; }

        internal TStream Stream<TStream, TStreamData>()
                where TStream : IStream<TStreamData>
                where TStreamData : IStreamData
        {
            var stream = Streams
                .Where(stream => stream is TStream)
                .Cast<TStream>()
                .SingleOrDefault();

            //https://stackoverflow.com/a/864860/1132646
            if (EqualityComparer<TStream>.Default.Equals(stream, default))
                throw new StreamNotAvailableForMicroException(typeof(TStreamData), ((IMicroFlow)this).Id);

            return stream;
        }
    }

    public abstract record LifeTimeScope
    {
        public record TransientStopped() : LifeTimeScope
        {
            public TransientWorking Start(IDisposable subscription) => new(subscription);
        }

        public record TransientWorking(IDisposable Subscription) : LifeTimeScope
        {
            public TransientStopped Stop()
            {
                Subscription.Dispose();

                return new TransientStopped();
            }
        }

        public record Singleton() : LifeTimeScope { }
    }
}
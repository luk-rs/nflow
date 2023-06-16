namespace Flow.Reactive
{

    using CustomExceptions;
    using Flow.Reactive.Streams.Middleware;
    using Services;
    using Streams;
    using Streams.Ephemeral;
    using Streams.Ephemeral.Commands;
    using Streams.Persisted;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Reactive.Threading.Tasks;
    using System.Runtime.CompilerServices;

    public interface IFlow : IDisposable
    {

        IObservable<Unit> Send<TCommand>(TCommand command, [CallerFilePath] string sender = "")
                where TCommand : Command;

        IObservable<TStreamData> Query<TStreamData>() where TStreamData : PersistedStreamData;

        TStreamData GetSnapshot<TStreamData>() where TStreamData : PersistedStreamData;

        IObservable<TStreamData> Listen<TStreamData>() where TStreamData : StreamData;

        [Obsolete("This method has been deprecated in favor of Query, Handle and Listen typed methods")]
        IObservable<TStreamData> ReadStreamData<TStreamData>() where TStreamData : IStreamData;

        void Start();

        bool Started { get; }

        void StartTransientMicro(string microId);

        void StopTransientMicro(string microId);

        IObservable<Exception> UnhandledException { get; }
    }


    internal interface IFlowMicro : IFlow
    {

        IEnumerable<IStream> Streams { get; }
        IObservable<TCommand> Handle<TCommand>() where TCommand : Command;

    }


    internal class MasterFlow : IFlowMicro
    {
        public IObservable<Unit> Send<TCommand>(TCommand command, [CallerFilePath] string sender = "")
                where TCommand : Command =>
            Observable
                .Return(CommandsStream)
                .Do(_ => _middlewares.ForEach(mw => mw.Intercept(command)))
                .Do(stream => stream.Notify(command))
                .Select(_ => Unit.Default);

        public IObservable<TStreamData> Query<TStreamData>()
                where TStreamData : PersistedStreamData
        {
            var stream = ((IFlowMicro)this).Streams
                                    .Where(stream => stream is IStream<TStreamData>)
                                    .Cast<IStream<TStreamData>>()
                                    .Select(stream => stream.Data)
                                    .SingleOrDefault();

            if (stream == default)
                throw new PublicStreamNotFoundException(typeof(TStreamData));

            return stream;
        }

        public TStreamData GetSnapshot<TStreamData>() where TStreamData : PersistedStreamData => 
            ((IFlowMicro)this).Streams
                .Where(stream => stream is IStream<TStreamData>)
                .Cast<IStream<TStreamData>>()
                .Select(stream => stream.Data)
                .SingleOrDefault()
                .Take(1)
                .ToTask()
                .Result;

        public IObservable<TStreamData> Listen<TStreamData>()
                where TStreamData : StreamData
        {
            var stream = ((IFlowMicro)this).Streams
                                    .Where(stream => stream is IStream<TStreamData>)
                                    .Cast<IStream<TStreamData>>()
                                    .Select(stream => stream.Data)
                                    .SingleOrDefault();

            if (stream == default)
                throw new PublicStreamNotFoundException(typeof(TStreamData));

            return stream;
        }

        [Obsolete("This method has been deprecated in favor of Query, Handle and Listen typed methods")]
        public IObservable<TStreamData> ReadStreamData<TStreamData>()
                where TStreamData : IStreamData
        {
            return ((IFlowMicro)this).Streams
                                .Where(stream => stream is IStream<TStreamData>)
                                .Cast<IStream<TStreamData>>()
                                .Select(stream => stream.Data)
                                .Single();
        }

        public IObservable<Exception> UnhandledException => _unhandledException;

        IEnumerable<IStream> IFlowMicro.Streams
        {
            get => Micros.Select(micro => micro.PublicStreams)
                    .SelectMany(stream => stream);
        }

        IObservable<TCommand> IFlowMicro.Handle<TCommand>()
        {
            return CommandsStream
             .Data
             .Where(command => command is TCommand)
             .Cast<TCommand>();
        }

        private List<IMiddleware> _middlewares;

        public MasterFlow(IEnumerable<IMicro> allMicros)
            : this(allMicros, false)
        { }

        public MasterFlow(IEnumerable<IMicro> allMicros, bool deferredStart)
        {
            CommandsStream = new CommandsStream();

            var micros = new List<IMicroFlow>();

            allMicros
                   .Cast<Micro>()
                   .ToList()
                   .ForEach(micro =>
                   {
                       micro.Flow = this;
                       micros.Add(micro);
                   });

            Micros = micros;

            if(Micros.Any())
                _middlewares = Micros.First().Middlewares.ToList();

            _middlewares
                .OfType<IFlowInjection>()
                .ToList()
                .ForEach(middleware => middleware.Flow = this);

            if (!deferredStart)
                Start();
        }

        public void Start()
        {
            if (Started)
                return;

            if (!_firstRun)
                ResetPersistedStreams();

            _firstRun = false;

            Started = true;

            Micros
                .Where(micro => ((Micro) micro).LifeTimeScope is LifeTimeScope.Singleton)
                .SelectMany(micro => micro.Nanos)
                .Select(nano => nano.Connect())
                .Merge()
                .Do(_ => { }, exception => Trace.WriteLine($"Flow Reactive Setup : {exception.GetType()} - {exception.Message} "), () => { })
                .Subscribe(
                    _ => { }, 
                    ex =>
                    {
                        Started = false;
                        _unhandledException.OnNext(ex);
                    }, 
                    () => { });
        }

        public void StartTransientMicro(string microId)
        {
            var micro = Micros.FirstOrDefault(micro => micro.Id == microId);

            if (micro is not Micro microService)
                throw new ArgumentException($"{microId} not found");

            microService.LifeTimeScope = ((Micro)micro).LifeTimeScope switch
            {
                LifeTimeScope.TransientStopped t => t.Start(ConnectNanos()),
                LifeTimeScope.TransientWorking => throw new InvalidOperationException($"{microId} is already started"),
                LifeTimeScope.Singleton => throw new InvalidOperationException($"{microId} is not transient")
            };

            Trace.WriteLine($"Transient Micro {microId} started");

            IDisposable ConnectNanos() =>
                micro
                    .Nanos
                    .Select(nano => nano.Connect())
                    .Merge()
                    .Subscribe();
        }

        public void StopTransientMicro(string microId)
        {
            var micro = Micros.FirstOrDefault(micro => micro.Id == microId);

            if (micro is not Micro microService)
                throw new ArgumentException($"{microId} not found");

            microService.LifeTimeScope = ((Micro)micro).LifeTimeScope switch
            {
                LifeTimeScope.TransientWorking t => t.Stop(),
                LifeTimeScope.TransientStopped => throw new InvalidOperationException($"{microId} is not started"),
                LifeTimeScope.Singleton => throw new InvalidOperationException($"{microId} is not transient")
            };

            ResetPersistedStreams(microId);

            Trace.WriteLine($"Transient Micro {microId} stopped");
        }

        private void ResetPersistedStreams() =>
            Micros
                .ToList()
                .ForEach(micro => micro.ResetPersistedStreams());

        private void ResetPersistedStreams(string microId) =>
            Micros
                .Single(micro => micro.Id == microId)
                .ResetPersistedStreams();

        public bool Started { get; private set; }

        public void Dispose() => 
            Micros.ToList().ForEach(micro => micro.Dispose());

        private CommandsStream CommandsStream { get; }

        private IEnumerable<IMicroFlow> Micros { get; }

        private bool _firstRun = true;

        private Subject<Exception> _unhandledException = new();          
    }
}
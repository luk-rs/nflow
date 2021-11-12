namespace nflow.core
{
    using System;
    using System.Reactive.Subjects;

    internal sealed class Oracle<TPersistedStream> where TPersistedStream : IPersistedStream, new()
    {
        private readonly BehaviorSubject<TPersistedStream> _stream = new(new TPersistedStream());

        public IObservable<TPersistedStream> Stream => _stream;

        public void Update(Action<TPersistedStream> with)
        {
            //TODO possible bottleneck/race condition here
            with(Value);
            _stream.OnNext(Value);
        }

        public TPersistedStream Value => _stream.Value;

        public void Dispose() => _stream.OnCompleted();
    }
}


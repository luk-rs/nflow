namespace streams.Core
{
    using System;
    using System.Reactive.Subjects;

    public sealed class PersistedStream<TStream> : IDisposable where TStream : IPersistedStream, new()
    {
        private readonly BehaviorSubject<TStream> _stream = new(new TStream());

        public IObservable<TStream> Stream => _stream;

        public void Update(Action<TStream> updateAction)
        {
            updateAction(Value);

            _stream.OnNext(Value);
        }

        public TStream Value => _stream.Value;

        public void Dispose() => _stream.OnCompleted();
    }
}


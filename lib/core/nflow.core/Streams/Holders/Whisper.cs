namespace nflow.core.Flow
{
    using System;
    using System.Reactive.Subjects;


    internal sealed class Whisper<TStream> where TStream : IStream, new()
    {
        private readonly Subject<TStream> _stream = new();

        public IObservable<TStream> Stream => _stream;

        public void Update(Func<TStream> updateAction)
        {
            _stream.OnNext(updateAction());
        }

        public void Dispose() => _stream.OnCompleted();
    }
}


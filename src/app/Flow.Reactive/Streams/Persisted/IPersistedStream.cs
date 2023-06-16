namespace Flow.Reactive.Streams.Persisted
{

    using System;


    public interface IPersistedStream<out TStreamData> : IStream<TStreamData>, IPersistedStream
            where TStreamData : IStreamData
    {

        TStreamData Update(Action<TStreamData> update);

        TStreamData InitialState { get; }

    }
    
    public interface IPersistedStream
    {
        void Reset();
    }
}
namespace Flow.Reactive.Streams
{

    using System;


    public interface IStream : IDisposable
    {

        bool Public { get; }

    }


    public interface IStream<out TStreamData> : IStream
            where TStreamData : IStreamData
    {

        IObservable<TStreamData> Data { get; }

    }

}
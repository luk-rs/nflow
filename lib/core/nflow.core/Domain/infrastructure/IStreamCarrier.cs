namespace nflow.core
{
    using System;

    public interface IStreamCarrier
    {
        bool Carrying<TOtherStream>()
        where TOtherStream : IStream;

        bool Carrying<TOtherStream>(TOtherStream stream)
        where TOtherStream : IStream;
        IObservable<TTargetStream> Hook<TTargetStream>() where TTargetStream : IStream;
        void Route(object payload);
        TTargetStream Value<TTargetStream>() where TTargetStream : IStream;
    }
}


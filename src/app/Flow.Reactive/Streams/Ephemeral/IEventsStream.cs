namespace Flow.Reactive.Streams.Ephemeral
{

    public interface IEventsStream<TStreamData> : IStream<TStreamData>
            where TStreamData : IStreamData
    {

        TStreamData Notify(TStreamData newData);

    }

}
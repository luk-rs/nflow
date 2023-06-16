namespace Flow.Reactive.Persistence.Concrete
{

    using Streams;


    public class InMemory : IRepository
    {
        public TStreamData Load<TStreamData>(TStreamData defaultData) where TStreamData : IStreamData
            => defaultData;

        public TStreamData Save<TStreamData>(TStreamData streamData) where TStreamData : IStreamData
            => streamData;

    }

}
namespace Flow.Reactive.Extensions
{
    using Flow.Reactive.Streams.Persisted.Table;

    public static class StreamExtensions
    {
        public static FlowTableStreamData<TStreamData, TKey, TData> CreateFlowTable<TStreamData, TKey, TData>(this IFlow flow, TKey key)
            where TStreamData : TablePersistedData<TKey, TData> =>
            new FlowTableStreamData<TStreamData, TKey, TData>(flow, key);
    }
}

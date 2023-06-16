namespace Flow.Reactive.Streams.Persisted.Table
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;

    public class FlowTableStreamData<TStreamData, TKey, TData>
        where TStreamData : TablePersistedData<TKey, TData>
    {
        private readonly IFlow _flow;
        private readonly TKey _key;

        public FlowTableStreamData(IFlow flow, TKey key)
        {
            _flow = flow;
            _key = key;
        }

        public IObservable<TData> Query() =>
            _flow
                .Query<TStreamData>()
                .Select(table => table switch
                {
                    _ when table.ContainsKey(_key) && !table.UpdatedKeys.Contains(_key) => Observable.Return(table.GetData(_key)),
                    _ => Observable.Empty<TData>()
                })
                .Take(1)
                .Switch()
                .Concat(_flow.Query<TStreamData>()
                             .Where(t => t.UpdatedKeys.Contains(_key) && t.ContainsKey(_key))
                             .Select(t => t.GetData(_key)));
    }
}

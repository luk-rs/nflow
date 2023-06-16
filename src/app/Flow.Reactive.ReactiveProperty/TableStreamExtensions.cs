namespace Flow.Reactive.ReactiveProperty
{

    using Flow.Reactive.Streams.Persisted.Table;
    using global::Reactive.Bindings;
    using Rx.Extensions;
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;


    public static class TableStreamExtensions
    {
        public static ReactiveProperty<T> Bind<T, TStreamData, TKey, TData>(this FlowTableStreamData<TStreamData, TKey, TData> tableStream,
                                                                            Func<TData, T> selector,
                                                                            CompositeDisposable disposables)
            where TStreamData : TablePersistedData<TKey, TData>
            => tableStream
                    .Query()
                    .Select(selector)
                    .ToReactiveProperty()
                    .AddToDisposables(disposables);

        public static ReactiveProperty<T> Bind<T, TStreamData, TKey, TData>(this FlowTableStreamData<TStreamData, TKey, TData> tableStream,
                                                                            Func<TData, T> selector,
                                                                            T initialValue,
                                                                            CompositeDisposable disposables)
            where TStreamData : TablePersistedData<TKey, TData> 
            => tableStream
                    .Query()
                    .Select(selector)
                    .ToReactiveProperty(initialValue)
                    .AddToDisposables(disposables);
    }
}

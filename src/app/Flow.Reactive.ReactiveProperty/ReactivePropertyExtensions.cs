namespace Flow.Reactive.ReactiveProperty
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using Flow.Reactive.Extensions;
    using Flow.Reactive.Streams.Ephemeral.Commands;
    using global::Reactive.Bindings;
    using Rx.Extensions;
    using Streams.Persisted;


    public static class ReactivePropertyExtensions
    {
        public static ReactiveProperty<T> Bind<T, TStreamData>(this IFlow flow,
                                                               Func<TStreamData, T> selector,
                                                               CompositeDisposable disposables)
            where TStreamData : PersistedStreamData
            => Bind(flow, selector, default, disposables);

        public static ReactiveProperty<T> Bind<T, TStreamData>(this IFlow flow,
                                                               Func<TStreamData, T> selector,
                                                               T initialValue,
                                                               CompositeDisposable disposables)
          where TStreamData : PersistedStreamData
          => flow
              .Query<TStreamData>()
              .Select(selector)
              .ToReactiveProperty(initialValue)
              .AddToDisposables(disposables);

        public static ReactiveProperty<T> Bind<T, TStreamData>(this IFlow flow,
                                                               Func<TStreamData, IObservable<T>> selector,
                                                               CompositeDisposable disposables)
         where TStreamData : PersistedStreamData
         => flow
             .Query<TStreamData>()
             .Select(selector)
             .Switch()
             .ToReactiveProperty()
             .AddToDisposables(disposables);

        public static ReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source, CompositeDisposable disposables) =>
            source.ToReactiveProperty().AddToDisposables(disposables);

        public static ReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source, T initialValue, CompositeDisposable disposables) =>
           source.ToReactiveProperty(initialValue).AddToDisposables(disposables);

        public static ReactiveProperty<T> SendCommand<T, TCommand>(this ReactiveProperty<T> property,
                                                                   IFlow flow,
                                                                   Func<T, TCommand> command,
                                                                   CompositeDisposable disposables)
            where TCommand : Command
        {
            property
                .Skip(1)
                .SendCommand(flow, command)
                .Subscribe()
                .AddToDisposables(disposables);

            return property;
        }
    }
}

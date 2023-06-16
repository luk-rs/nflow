namespace Flow.Rx.Extensions
{

    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;


    public static class RxExtensions
    {
        public static IObservable<(T Item, NotifyCollectionChangedAction Action)> ToAddRemoveObservable<T>(this ObservableCollection<T> source)
        {
            return Observable
                .FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    handler => (sender, args) => handler(args),
                    handler => source.CollectionChanged += handler,
                    handler => source.CollectionChanged -= handler)
                .Select(args =>
                {
                    switch (args.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            foreach (var newItem in args.NewItems.Cast<T>())
                            {
                                return (newItem, NotifyCollectionChangedAction.Add);
                            }
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            foreach (var oldItem in args.OldItems.Cast<T>())
                            {
                                return (oldItem, NotifyCollectionChangedAction.Remove);
                            }
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            foreach (var newItem in args.NewItems.Cast<T>())
                            {
                                return (newItem, NotifyCollectionChangedAction.Add);
                            }
                            foreach (var oldItem in args.OldItems.Cast<T>())
                            {
                                return (oldItem, NotifyCollectionChangedAction.Remove);
                            }
                            break;
                    }

                    Debug.Assert(false, "Unexpected action reached");
                    return (default(T), NotifyCollectionChangedAction.Reset);
                });
        }

        public static IObservable<T> ThrottleMax<T>(this IObservable<T> source,
                                                    TimeSpan dueTime,
                                                    TimeSpan maxTime)
            => source.ThrottleMax(dueTime, maxTime, Scheduler.Default);

        public static IObservable<T> ThrottleMax<T>(this IObservable<T> source,
                                                    TimeSpan dueTime,
                                                    TimeSpan maxTime,
                                                    IScheduler scheduler)
        {
            return Observable.Create<T>(o =>
            {
                var hasValue = false;
                T value = default;

                var maxTimeDisposable = new SerialDisposable();
                var dueTimeDisposable = new SerialDisposable();

                void action()
                {
                    if (hasValue)
                    {
                        maxTimeDisposable.Disposable = Disposable.Empty;
                        dueTimeDisposable.Disposable = Disposable.Empty;
                        o.OnNext(value);
                        hasValue = false;
                    }
                }

                return source.Subscribe(
                    x =>
                    {
                        if (!hasValue)
                            maxTimeDisposable.Disposable = scheduler.Schedule(maxTime, action);

                        hasValue = true;
                        value = x;
                        dueTimeDisposable.Disposable = scheduler.Schedule(dueTime, action);
                    },
                    o.OnError,
                    o.OnCompleted
                );
            });
        }

        public static T AddToDisposables<T>(this T obj, CompositeDisposable compositeDisposable)
        {
            if (obj is IDisposable cast)
                compositeDisposable.Add(cast);

            return obj;
        }

        public static IObservable<T> ObserveLatestOn<T>(this IObservable<T> source, IScheduler scheduler)
        {
            return Observable.Create<T>(observer =>
            {
                Notification<T> outsideNotification = null;
                var gate = new object();
                var active = false;
                var cancelable = new MultipleAssignmentDisposable();
                var disposable = source.Materialize().Subscribe(thisNotification =>
                {
                    bool wasNotAlreadyActive;
                    lock (gate)
                    {
                        wasNotAlreadyActive = !active;
                        active = true;
                        outsideNotification = thisNotification;
                    }

                    if (wasNotAlreadyActive)
                    {
                        cancelable.Disposable = scheduler.Schedule(self =>
                        {
                            Notification<T> localNotification = null;
                            lock (gate)
                            {
                                localNotification = outsideNotification;
                                outsideNotification = null;
                            }
                            localNotification.Accept(observer);
                            var hasPendingNotification = false;
                            lock (gate)
                            {
                                hasPendingNotification = active = outsideNotification != null;
                            }
                            if (hasPendingNotification)
                                self();
                        });
                    }
                });
                return new CompositeDisposable(disposable, cancelable);
            });
        }

        public static IObservable<TOutput> SwitchSelect<TInput, TOutput>(this IObservable<TInput> source, Func<TInput, IObservable<TOutput>> selector)
           => source
             .Select(selector)
             .Switch();

        public static IObservable<TOutput> ConcatSelect<TInput, TOutput>(this IObservable<TInput> source, Func<TInput, IObservable<TOutput>> selector)
           => source
             .Select(selector)
             .Concat();

        public static IObservable<TOutput> MergeSelect<TInput, TOutput>(this IObservable<TInput> source, Func<TInput, IObservable<TOutput>> selector)
          => source
            .Select(selector)
            .Merge();

        public static IDisposable Subscribe<T>(this IObservable<T> source, CompositeDisposable disposables) =>
            source
                .Subscribe()
                .AddToDisposables(disposables);

    }
}
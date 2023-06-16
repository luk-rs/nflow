namespace Flow.Reactive.IPC.Middleware
{
    using Flow.Reactive;
    using Flow.Reactive.IPC.IPCMicroService.Commands;
    using Flow.Reactive.IPC.IPCMicroService.Streams.Private;
    using Flow.Reactive.Streams;
    using Flow.Reactive.Streams.Ephemeral;
    using Flow.Reactive.Streams.Ephemeral.Commands;
    using Flow.Reactive.Streams.Middleware;
    using Flow.Reactive.Streams.Persisted;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class DispatcherMiddleware : IMiddleware, IFlowInjection
    {
        private Dictionary<Type, object> _lastQueryValues = new();

        private Dictionary<Type, List<string>> _subscriptions = new();

        public IFlow Flow { get; set; }

        public TStreamData Intercept<TStreamData>(TStreamData data) where TStreamData : IStreamData => 
            data switch
            {
                SubscriptionItem newSubscription => HandleNewSubscription(data, newSubscription),
                PersistedStreamData => UpdateCacheAndNotify(data),
                StreamData streamData when streamData is not Command => Notify(data),
                _ => data
            };

        private TStreamData HandleNewSubscription<TStreamData>(TStreamData data, SubscriptionItem subscription) 
            where TStreamData : IStreamData
        {
            if (subscription.Publisher != Assembly.GetEntryAssembly().GetName().Name)
                return data;

            if (_subscriptions.TryGetValue(subscription.Type, out var subscribers))
                subscribers.Add(subscription.Subscriber);
            else
                _subscriptions.Add(subscription.Type, new List<string>(new[] { subscription.Subscriber }));

            if (IPCConfigurator.IsPersistedStreamData(subscription.Type) && _lastQueryValues.ContainsKey(subscription.Type))
                Flow
                    .Send(new SendStreamData(subscription.Subscriber, _lastQueryValues[subscription.Type]))
                    .Subscribe();

            return data;
        }

        private TStreamData UpdateCacheAndNotify<TStreamData>(TStreamData query)
            where TStreamData : IStreamData
        {
            _lastQueryValues[query.GetType()] = query;

            return Notify(query);
        }

        private TStreamData Notify<TStreamData>(TStreamData data) where TStreamData : IStreamData
        {
            if (_subscriptions.TryGetValue(data.GetType(), out var subscribers))
                subscribers
                    .ForEach(subscriber => Flow
                                            .Send(new SendStreamData(subscriber, data))
                                            .Subscribe());

            return data;
        }
    }
}

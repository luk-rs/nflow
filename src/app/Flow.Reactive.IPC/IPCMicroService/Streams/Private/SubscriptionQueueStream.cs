namespace Flow.Reactive.IPC.IPCMicroService.Streams.Private
{
    using Flow.Reactive.Streams.Ephemeral;
    using System;

    public class SubscriptionQueueStream : EventsStream<SubscriptionItem>
    {
    }

    public class SubscriptionItem : StreamData
    {
        public SubscriptionItem(string subscriber, string publisher, Type type)
        {
            Subscriber = subscriber;
            Publisher = publisher;
            Type = type;
        }

        public string Subscriber { get; }

        public string Publisher { get; }

        public Type Type { get; }
    }
}

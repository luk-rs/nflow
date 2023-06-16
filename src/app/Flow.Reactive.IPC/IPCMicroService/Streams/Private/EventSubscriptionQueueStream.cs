namespace Flow.Reactive.IPC.IPCMicroService.Streams.Private
{
    using Flow.Reactive.Streams.Ephemeral;
    using System;

    public class EventSubscriptionQueueStream : EventsStream<EventSubscriptionItem>
    {

    }

    public class EventSubscriptionItem : StreamData
    {
        public EventSubscriptionItem(Type streamType) => StreamType = streamType;

        public Type StreamType { get; }
    }
}

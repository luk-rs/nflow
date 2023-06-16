namespace Flow.Reactive.Tests.FlowTests.Core.MicroServices.TransientMicro.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.Streams.Public;
    using Flow.Reactive.Tests.FlowTests.Core.MicroServices.TransientMicro.Streams.Public;
    using System;
    using System.Reactive;

    public class Listener : EventListenerNano<EventStream1Data>
    {
        public override IObservable<Unit> Connect() =>
            Listen
                .Update<EventStream1Data, TransientPersistedStream1Data>(this,
                (@event, stream) => stream.Count = @event.Count);
    }
}

namespace Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.Commands;
    using Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.Streams.Public;
    using System;
    using System.Reactive;

    public class Command1Handler : HandlerNano<Command1>
    {
        public override IObservable<Unit> Connect() =>
            Handle
                .Update<Command1, PersistedStream1Data>(this, (_, stream) => stream.Count++);
    }
}

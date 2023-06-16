namespace Flow.Reactive.Tests.FlowTests.SampleMicro.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;
    using System;
    using System.Reactive;

    public class CommandBHandler : HandlerNano<CommandB>
    {
        public override IObservable<Unit> Connect() =>
             Handle
            .Notify(this, _ => new EventStreamData1());
    }
}

namespace Flow.Reactive.Tests.FlowTests.SampleMicro.NanoServices
{
    using System;
    using System.Reactive;
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;

    public class CommandAHandler : HandlerNano<CommandA>
    {
        public override IObservable<Unit> Connect() =>
            Handle
                .Update<CommandA, PersistedStreamData1>(this, (command, stream) => stream.UpdateCount++);
    }
}

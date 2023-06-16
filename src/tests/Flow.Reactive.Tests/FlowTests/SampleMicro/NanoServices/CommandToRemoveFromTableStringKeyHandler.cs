namespace Flow.Reactive.Tests.FlowTests.SampleMicro.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;
    using System;
    using System.Reactive;

    public class CommandToRemoveFromTableStringKeyHandler : HandlerNano<CommandToRemoveFromTableStringKey>
    {
        public override IObservable<Unit> Connect() =>
            Handle
                .Update<CommandToRemoveFromTableStringKey, TableStringKey>(this,
                    (command, table) => table.Remove(command.Key));
    }
}

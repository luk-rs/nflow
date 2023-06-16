namespace Flow.Reactive.Tests.FlowTests.SampleMicro.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;
    using System;
    using System.Reactive;

    public class CommandToUpdateRecordInTableStringKeyHandler : HandlerNano<CommandToUpdateRecordInTableStringKey>
    {
        public override IObservable<Unit> Connect() =>
            Handle
            .Update<CommandToUpdateRecordInTableStringKey, TableStringKey>(this,
                (command, table) => table.UpdateOrInsert(command.Key, new RecordData(command.NewValue)));
    }
}

namespace Flow.Reactive.Tests.FlowTests.SampleMicro.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;
    using System;
    using System.Reactive;

    public class CommandToUpdateRecordInTableHandler : HandlerNano<CommandToUpdateRecordInTable>
    {
        public override IObservable<Unit> Connect() =>
            Handle
            .Update<CommandToUpdateRecordInTable, Table>(this,
                (command, table) => table.UpdateOrInsert(command.RecordId, new RecordData(command.NewValue)));
    }
}

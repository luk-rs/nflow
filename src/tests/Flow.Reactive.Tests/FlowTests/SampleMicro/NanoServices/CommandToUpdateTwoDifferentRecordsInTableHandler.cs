namespace Flow.Reactive.Tests.FlowTests.SampleMicro.NanoServices
{
    using System;
    using System.Reactive;
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;

    public class CommandToUpdateTwoDifferentRecordsInTableHandler : HandlerNano<CommandToUpdateTwoDifferentRecordsInTable>
    {
        public override IObservable<Unit> Connect() =>
            Handle
                .UpdateAnd<CommandToUpdateTwoDifferentRecordsInTable, Table>(this,
                    (command, table) => table.UpdateOrInsert(command.RecordId1, new RecordData(command.NewValue1)))
                .Update<CommandToUpdateTwoDifferentRecordsInTable, Table>(this,
                    (command, table) => table.UpdateOrInsert(command.RecordId2, new RecordData(command.NewValue2)));
    }
}

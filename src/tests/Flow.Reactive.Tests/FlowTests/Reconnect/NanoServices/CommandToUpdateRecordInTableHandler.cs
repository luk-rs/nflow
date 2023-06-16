namespace Flow.Reactive.Tests.FlowTests.Reconnect.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.Reconnect.Commands;
    using Flow.Reactive.Tests.FlowTests.Reconnect.Streams.Public;
    using System;
    using System.Reactive;

    public class CommandToUpdateRecordInTableHandler : HandlerNano<CommandToUpdateRecordInTable>
    {
        public override IObservable<Unit> Connect() =>
            Handle
            .Update<CommandToUpdateRecordInTable, ReconnectableTable>(this,
                (command, table) => table.UpdateOrInsert(command.RecordId, new RecordData(command.NewValue)));
    }
}

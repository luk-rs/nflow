namespace Flow.Reactive.Tests.FlowTests.SampleMicro.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;
    using System;
    using System.Reactive;

    public class CommandToUpdateAllRecordsInTableHandler : HandlerNano<CommandToUpdateAllRecordsInTable>
    {
        public override IObservable<Unit> Connect() =>
            Handle
            .Update<CommandToUpdateAllRecordsInTable, Table>(this, (command, table) =>
                table.UpdateAll(new RecordData(command.NewValue)));
    }
}

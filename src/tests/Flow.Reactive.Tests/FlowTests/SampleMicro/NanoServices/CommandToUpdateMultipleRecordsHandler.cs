namespace Flow.Reactive.Tests.FlowTests.SampleMicro.NanoServices
{
    using System;
    using System.Linq;
    using System.Reactive;
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;

    public class CommandToUpdateMultipleRecordsHandler : HandlerNano<CommandToUpdateMultipleRecords>
    {
        public override IObservable<Unit> Connect() =>
            Handle
                .Update<CommandToUpdateMultipleRecords, Table>(this,
                (command, table) => table.UpdateMultiple(command.NewValues.Select(x => (x.RecordId, new RecordData(x.NewValue))).ToList()));
    }
}

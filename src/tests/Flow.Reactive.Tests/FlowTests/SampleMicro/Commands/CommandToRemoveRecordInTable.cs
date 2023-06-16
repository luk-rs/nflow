namespace Flow.Reactive.Tests.FlowTests.SampleMicro.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;

    public class CommandToRemoveRecordInTable : Command
    {
        public CommandToRemoveRecordInTable(int recordId) => RecordId = recordId;

        public int RecordId { get; }
    }
}

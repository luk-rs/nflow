namespace Flow.Reactive.Tests.FlowTests.SampleMicro.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;

    public class CommandToUpdateRecordInTable : Command
    {
        public CommandToUpdateRecordInTable(int recordId, string newValue)
        {
            RecordId = recordId;
            NewValue = newValue;
        }

        public int RecordId { get; }

        public string NewValue { get; }
    }
}

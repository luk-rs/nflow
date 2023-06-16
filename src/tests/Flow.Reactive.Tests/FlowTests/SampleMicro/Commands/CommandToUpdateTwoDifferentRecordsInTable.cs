namespace Flow.Reactive.Tests.FlowTests.SampleMicro.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;

    public class CommandToUpdateTwoDifferentRecordsInTable : Command
    {
        public CommandToUpdateTwoDifferentRecordsInTable(int recordId1, string newValue1, int recordId2, string newValue2)
        {
            RecordId1 = recordId1;
            NewValue1 = newValue1;
            RecordId2 = recordId2;
            NewValue2 = newValue2;
        }

        public int RecordId1 { get; }

        public string NewValue1 { get; }

        public int RecordId2 { get; }
        public string NewValue2 { get; }
    }
}

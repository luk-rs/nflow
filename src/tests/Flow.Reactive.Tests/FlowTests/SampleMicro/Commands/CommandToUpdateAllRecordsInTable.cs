namespace Flow.Reactive.Tests.FlowTests.SampleMicro.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;

    public class CommandToUpdateAllRecordsInTable : Command
    {
        public CommandToUpdateAllRecordsInTable(string newValue)
        {
            NewValue = newValue;
        }

        public string NewValue { get; }
    }
}

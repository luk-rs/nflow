namespace Flow.Reactive.Tests.FlowTests.SampleMicro.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;
    using System.Collections.Generic;

    public class CommandToUpdateMultipleRecords : Command
    {
        public CommandToUpdateMultipleRecords(IEnumerable<(int RecordId, string NewValue)> newValues)
        {
            NewValues = newValues;
        }

        public IEnumerable<(int RecordId, string NewValue)> NewValues { get; }
    }
}

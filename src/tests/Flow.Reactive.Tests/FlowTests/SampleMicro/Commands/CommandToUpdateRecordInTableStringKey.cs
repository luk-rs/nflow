namespace Flow.Reactive.Tests.FlowTests.SampleMicro.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;

    public class CommandToUpdateRecordInTableStringKey : Command
    {
        public CommandToUpdateRecordInTableStringKey(string key, string newValue)
        {
            Key = key;
            NewValue = newValue;
        }

        public string Key { get; }

        public string NewValue { get; }
    }
}

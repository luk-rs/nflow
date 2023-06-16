namespace Flow.Reactive.Tests.FlowTests.SampleMicro.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;

    public class CommandToRemoveFromTableStringKey : Command
    {
        public CommandToRemoveFromTableStringKey(string key) => Key = key;

        public string Key { get; }
    }
}

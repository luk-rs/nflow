namespace Flow.Nanos.Lab.MicroService.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;

    public class CommandToRaiseEvent : Command
    {
        public CommandToRaiseEvent(int eventValue) => EventValue = eventValue;

        public int EventValue { get; }
    }
}

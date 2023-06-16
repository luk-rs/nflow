namespace Flow.Castle.Windsor.Domain.Micro2.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;

    public class UpdateA2 : Command
    {

        public int Value { get; }

        public UpdateA2(int value)
        {
            Value = value;
        }

    }

}
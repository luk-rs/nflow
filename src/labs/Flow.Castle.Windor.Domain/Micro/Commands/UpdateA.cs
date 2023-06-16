namespace Flow.Castle.Windsor.Domain.Micro.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;

    public class UpdateA : Command
    {

        public int Value { get; }

        public UpdateA(int value)
        {
            Value = value;
        }

    }

}
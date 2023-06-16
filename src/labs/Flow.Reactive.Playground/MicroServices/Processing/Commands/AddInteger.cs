namespace Flow.Reactive.Playground.MicroServices.Processing.Commands
{

    using Reactive.Streams.Ephemeral.Commands;


    public class AddInteger : Command
    {
        public AddInteger(int integer)
        {
            Integer = integer;
        }

        public int Integer { get; }

        public override string ShortFormat => $"Add Integer {Integer} Handled";
    }
}

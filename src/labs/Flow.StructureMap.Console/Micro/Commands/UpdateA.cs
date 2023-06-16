namespace Flow.StructureMap.Console.Micro.Commands
{

    using Reactive.Streams.Ephemeral.Commands;


    internal class UpdateA : Command
    {

        public int Value { get; }

        public UpdateA(int value)
        {
            Value = value;
        }

    }

}
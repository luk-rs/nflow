namespace Flow.Reactive.Streams.Ephemeral.Commands
{

    using Ephemeral;


    public abstract class Command : StreamData
    {
        public override bool Trace { get; set; } = false;
    }
}
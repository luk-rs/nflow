namespace Flow.Reactive.Streams.Ephemeral
{

    public abstract class StreamData : IStreamData
    {

        public virtual string ShortFormat => GetType().Name;

        public virtual bool Trace { get; set; } = true;
    }

}
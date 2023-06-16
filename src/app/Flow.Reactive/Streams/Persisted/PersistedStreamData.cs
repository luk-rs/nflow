namespace Flow.Reactive.Streams.Persisted
{
    using Newtonsoft.Json;

    public abstract class PersistedStreamData : IStreamData
    {

        public virtual IStreamData Clone() => (IStreamData)MemberwiseClone();

        [JsonIgnore]
        public virtual string ShortFormat => GetType().Name;

        public virtual bool Trace => true;

    }
}
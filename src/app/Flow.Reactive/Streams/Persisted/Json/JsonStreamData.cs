namespace Flow.Reactive.Streams.Persisted.Json
{

    using Newtonsoft.Json;


    public abstract class JsonStreamData : PersistedStreamData
    {

        public sealed override IStreamData Clone() => (IStreamData)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(this), GetType());

    }
}
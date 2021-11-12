namespace streams.MicroServiceA.Streams
{
    using nflow.core;

    public class Bar : IPersistedStream
    {
        public int SomeValue { get; set; } = 0;
    }
}

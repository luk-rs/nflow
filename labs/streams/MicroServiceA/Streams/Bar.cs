namespace streams.MicroServiceA.Streams
{
    using nflow.core.Flow;
    using streams.Core;

    public class Bar : IPersistedStream
    {
        public int SomeValue { get; set; } = 0;
    }
}

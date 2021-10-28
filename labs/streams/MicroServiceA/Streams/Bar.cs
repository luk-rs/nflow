namespace streams.MicroServiceA.Streams
{
    using streams.Core;

    public class Bar : IPersistedStream
    {
        public int SomeValue { get; set; } = 0;
    }
}

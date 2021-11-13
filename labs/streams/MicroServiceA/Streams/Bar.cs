namespace streams.MicroServiceA.Streams
{
    using nflow.core;

    public class Bar : IWhisper
    {
        public int SomeValue { get; set; } = 0;
    }
}

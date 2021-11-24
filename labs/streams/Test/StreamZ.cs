namespace streams.Test.Streams
{
    using nflow.core;

    public record StreamZ(string Name, int Rand) : IWhisper
    {
        bool IStream.IsPublic => true;

    }
    public record PStreamZ : IOracle
    {
        bool IStream.IsPublic => true;
        public int Age { get; set; }

    }
}
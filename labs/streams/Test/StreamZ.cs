namespace streams.Test.Streams
{
    using nflow.core;

    public record StreamZ : IWhisper
    {
        bool IStream.IsPublic => true;
        public string Name { get; set; }

    }
}
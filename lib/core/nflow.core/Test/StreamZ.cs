namespace nflow.core.Test.Streams
{

    public record StreamZ : IWhisper
    {
        bool IStream.IsPublic => true;
        public string Name { get; set; }

    }

    public record CommandZ : ICommand
    {
        public string Name { get; set; }
    }
}
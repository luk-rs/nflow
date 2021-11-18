namespace streams.Test.Commands
{
    using nflow.core;

    public record CommandZ : ICommand
    {
        public string Name { get; set; }
    }
}
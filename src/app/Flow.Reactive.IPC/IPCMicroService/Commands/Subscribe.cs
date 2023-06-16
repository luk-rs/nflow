namespace Flow.Reactive.IPC.IPCMicroService.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;

    public class Subscribe : Command
    {
        public Subscribe(string receiver, string streamTypeName)
        {
            Receiver = receiver;
            StreamTypeName = streamTypeName;
        }

        public string Receiver { get; }

        public string StreamTypeName { get; }
    }
}

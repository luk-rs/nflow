namespace Flow.Reactive.IPC.IPCMicroService.Commands
{
    using Flow.Reactive.IPC;
    using Flow.Reactive.Streams.Ephemeral.Commands;
    using Newtonsoft.Json;

    public class IPCCommand : Command
    {
        public IPCCommand(string receiver, object command)
        {
            Command = new JsonMessage(IPCConfigurator.AssemblyName, MessageType.Command, command.GetType().Name, JsonConvert.SerializeObject(command));
            Receiver = receiver;
        }

        public JsonMessage Command { get; }

        public string Receiver { get; }
    }
}

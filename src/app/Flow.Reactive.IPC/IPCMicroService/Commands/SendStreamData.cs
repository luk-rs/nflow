namespace Flow.Reactive.IPC.IPCMicroService.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;
    using Newtonsoft.Json;

    public class SendStreamData : Command
    {
        public SendStreamData(string receiver, object streamData)
        {
            Receiver = receiver;
            StreamData = new JsonMessage(IPCConfigurator.AssemblyName, MessageType.StreamData, streamData.GetType().Name, JsonConvert.SerializeObject(streamData));
        }

        public string Receiver { get; }

        public JsonMessage StreamData { get; }
    }
}

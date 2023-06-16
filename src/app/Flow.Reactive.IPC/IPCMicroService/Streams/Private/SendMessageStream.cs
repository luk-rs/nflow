namespace Flow.Reactive.IPC.IPCMicroService.Streams.Private
{
    using Flow.Reactive.IPC;
    using Flow.Reactive.Streams.Ephemeral;

    public class SendMessageStream : EventsStream<SendMessage>
    {
    }

    public class SendMessage : StreamData
    {
        public SendMessage(string receiver, JsonMessage message)
        {
            Receiver = receiver;
            Message = message;
        }

        public string Receiver { get; }

        public JsonMessage Message { get; }

        public override string ShortFormat => $"Send {Message.Type} from {Message.Sender} to {Receiver} {Message.TypeName}";
    }
}

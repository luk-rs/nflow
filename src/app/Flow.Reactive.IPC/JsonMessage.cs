namespace Flow.Reactive.IPC
{
    public class JsonMessage
    {
        public JsonMessage(string sender, MessageType type, string typeName, string content)
        {
            Sender = sender;
            Type = type;
            TypeName = typeName;
            Content = content;
        }

        public string Sender { get; }

        public MessageType Type { get; }

        public string TypeName { get; }

        public string Content { get; }
    }

    public enum MessageType
    {
        Command,
        Subscription,
        StreamData
    }
}

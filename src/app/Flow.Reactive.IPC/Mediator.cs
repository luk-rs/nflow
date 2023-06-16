namespace Flow.Reactive.IPC
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Pipes;
    using System.Reactive.Subjects;

    public interface IMediator
    {
        IObservable<JsonMessage> NewMessage { get; }

        string GetPipeName(string serverName, string clientName);

        void AddOutputPipe(string clientName, NamedPipeServerStream pipe);

        void AddInputPipe(string serverName, NamedPipeClientStream pipe);

        void SendMessage(string clientName, JsonMessage message);
    }

    public class Mediator : IMediator
    {
        private Dictionary<string, (NamedPipeServerStream Pipe, StreamWriter StreamWriter)> _outputPipes = new();
        private Dictionary<string, NamedPipeClientStream> _inputPipes = new();

        private Subject<JsonMessage> _newMessage = new();

        public IObservable<JsonMessage> NewMessage => _newMessage;

        public string GetPipeName(string serverName, string clientName) =>
            $"{serverName}_{clientName}";

        public void AddOutputPipe(string clientName, NamedPipeServerStream pipe) =>
            _outputPipes.Add(clientName, (pipe, new StreamWriter(pipe)));

        public void AddInputPipe(string serverName, NamedPipeClientStream pipe)
        {
            _inputPipes.Add(serverName, pipe);

            var reader = new StreamReader(pipe);

            while (true)
            {
                var input = reader.ReadLine();

                if (string.IsNullOrEmpty(input))
                    break;

                var message = JsonConvert.DeserializeObject<JsonMessage>(input);
                _newMessage.OnNext(message);
            }
        }

        public void SendMessage(string clientName, JsonMessage message)
        {
            var streamWriter = _outputPipes[clientName].StreamWriter;
            streamWriter.WriteLine(JsonConvert.SerializeObject(message));
            streamWriter.Flush();
        }
    }
}

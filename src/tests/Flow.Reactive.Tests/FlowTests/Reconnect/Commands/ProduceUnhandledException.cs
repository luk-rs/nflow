namespace Flow.Reactive.Tests.FlowTests.Reconnect.Commands
{
    using Flow.Reactive.Streams.Ephemeral.Commands;
    using System;

    public class ProduceUnhandledException : Command
    { 
        public ProduceUnhandledException(Exception exception) => Exception = exception;

        public Exception Exception { get; }
    }
}

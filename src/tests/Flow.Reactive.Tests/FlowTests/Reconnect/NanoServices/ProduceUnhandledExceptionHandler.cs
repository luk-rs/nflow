namespace Flow.Reactive.Tests.FlowTests.Reconnect.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.Reconnect.Commands;
    using System;
    using System.Reactive;

    public class ProduceUnhandledExceptionHandler : HandlerNano<ProduceUnhandledException>
    {
        public override IObservable<Unit> Connect() =>
            Handle
            .PerformAction(command => throw command.Exception);
    }
}

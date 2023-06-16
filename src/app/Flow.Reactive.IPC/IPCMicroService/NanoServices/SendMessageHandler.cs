namespace Flow.Reactive.IPC.IPCMicroService.NanoServices
{
    using Flow.Reactive.IPC.IPCMicroService.Streams.Private;
    using Flow.Reactive.Services;
    using System;
    using System.Reactive;

    public class SendMessageHandler : EventListenerNano<SendMessage>
    {
        private readonly IMediator _mediator;

        public SendMessageHandler(IMediator mediator) => _mediator = mediator;

        public override IObservable<Unit> Connect() =>
            Listen
                .PerformAction(message => _mediator.SendMessage(message.Receiver, message.Message));
    }
}

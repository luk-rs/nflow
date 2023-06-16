namespace Flow.Reactive.IPC.IPCMicroService.NanoServices
{
    using Flow.Reactive.Services;
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using Flow.Reactive.IPC.IPCMicroService.Streams.Private;

    public class SubscriptionProcessor : TriggerNano<JsonMessage>
    {
        private readonly IMediator _mediator;

        public SubscriptionProcessor(IMediator mediator) => _mediator = mediator;

        protected override IObservable<JsonMessage> Trigger =>
            _mediator
                .NewMessage
                .Where(message => message.Type == MessageType.Subscription);

        public override IObservable<Unit> Connect() =>
            Trigger
                .Notify(this, x => new SubscriptionItem(x.Sender, x.Content, IPCConfigurator.StreamTypes.First(type => type.Name == x.TypeName)));
    }
}

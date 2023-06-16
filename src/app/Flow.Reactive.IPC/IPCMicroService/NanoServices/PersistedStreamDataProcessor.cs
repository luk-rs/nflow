namespace Flow.Reactive.IPC.IPCMicroService.NanoServices
{
    using Flow.Reactive.Services;
    using Newtonsoft.Json;
    using System;
    using System.Reactive;
    using System.Reactive.Linq;

    public class PersistedStreamDataProcessor : TriggerNano<JsonMessage>
    {
        private readonly IMediator _mediator;

        public PersistedStreamDataProcessor(IMediator mediator) => _mediator = mediator;

        protected override IObservable<JsonMessage> Trigger =>
            _mediator
                .NewMessage
                .Where(message => message.Type == MessageType.StreamData)
                .Where(message => IPCConfigurator.IsPersistedStreamData(IPCConfigurator.GetStreamType(message.TypeName)));

        public override IObservable<Unit> Connect() =>
            Trigger
                .Update(this,
                        message => IPCConfigurator.GetStreamType(message.TypeName),
                        message => data => JsonConvert.DeserializeObject(message.Content, IPCConfigurator.GetStreamType(message.TypeName)));
    }
}

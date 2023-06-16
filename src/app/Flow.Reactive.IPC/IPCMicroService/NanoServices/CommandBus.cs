namespace Flow.Reactive.IPC.IPCMicroService.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Streams.Ephemeral.Commands;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;

    public class CommandBus : TriggerNano<JsonMessage>
    {
        private readonly IMediator _mediator;

        public CommandBus(IMediator mediator) => _mediator = mediator;

        protected override IObservable<JsonMessage> Trigger =>
           _mediator
                .NewMessage
                .Where(message => message.Type == MessageType.Command);

        public override IObservable<Unit> Connect() =>
            Trigger
                .SendCommand(this, item => 
                                    (Command)JsonConvert.DeserializeObject(item.Content, 
                                                                           IPCConfigurator
                                                                            .CommandTypes
                                                                            .First(type => type.Name == item.TypeName)));
    }
}

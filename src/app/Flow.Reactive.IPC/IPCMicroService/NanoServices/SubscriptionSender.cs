namespace Flow.Reactive.IPC.IPCMicroService.NanoServices
{
    using Flow.Reactive.IPC.IPCMicroService.Commands;
    using Flow.Reactive.IPC.IPCMicroService.Streams.Private;
    using Flow.Reactive.Services;
    using System;
    using System.Reactive;

    public class SubscriptionSender : HandlerNano<Subscribe>
    {
        public override IObservable<Unit> Connect() =>
           Handle
               .Notify(this, command => 
                       new SendMessage(command.Receiver, 
                                       new JsonMessage(IPCConfigurator.AssemblyName,
                                                       MessageType.Subscription, 
                                                       command.StreamTypeName,
                                                       command.Receiver)));
    }
}

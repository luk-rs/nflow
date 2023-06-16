namespace Flow.Reactive.IPC.IPCMicroService.NanoServices
{
    using Flow.Reactive.IPC.IPCMicroService.Commands;
    using Flow.Reactive.IPC.IPCMicroService.Streams.Private;
    using Flow.Reactive.Services;
    using System;
    using System.Reactive;

    public class CommandSender : HandlerNano<IPCCommand>
    {
        public override IObservable<Unit> Connect() =>
            Handle
                .Notify(this, command => new SendMessage(command.Receiver, command.Command));
    }
}

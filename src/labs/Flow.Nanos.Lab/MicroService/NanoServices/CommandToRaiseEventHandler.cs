namespace Flow.Nanos.Lab.MicroService.NanoServices
{
    using Flow.Nanos.Lab.MicroService.Commands;
    using Flow.Nanos.Lab.MicroService.Streams;
    using Flow.Reactive.Services;
    using System;
    using System.Reactive;

    public class CommandToRaiseEventHandler : HandlerNano<CommandToRaiseEvent>
    {
        public override IObservable<Unit> Connect() =>
            Handle.Notify(this, command => new MyEventPayload(command.EventValue));
    }
}

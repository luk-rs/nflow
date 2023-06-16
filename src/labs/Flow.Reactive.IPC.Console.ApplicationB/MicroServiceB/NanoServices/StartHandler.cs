namespace Flow.Reactive.IPC.Console.ApplicationB.MicroServiceB.NanoServices
{
    using System;
    using System.Reactive;
    using Flow.Reactive.IPC;
    using Flow.Reactive.IPC.Console.ApplicationB.MicroServiceB.Commands;
    using Flow.Reactive.Services;

    public class StartHandler : HandlerNano<Start>
    {
        public override IObservable<Unit> Connect() =>
            Handle
                .SendIPCCommand(this, "Flow.Reactive.IPC.Console.ApplicationA", _ => new CommandFoo());
    }
}

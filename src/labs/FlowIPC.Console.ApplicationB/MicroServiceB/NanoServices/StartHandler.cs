namespace FlowIPC.Console.ApplicationB.MicroServiceB.NanoServices
{
    using System;
    using System.Reactive;
    using Flow.Reactive.IPC;
    using Flow.Reactive.Services;
    using FlowIPC.Console.ApplicationB.MicroServiceB.Commands;

    public class StartHandler : HandlerNano<Start>
    {
        public override IObservable<Unit> Connect() =>
            Handle
                .SendIPCCommand(this, "FlowIPC.Console.ApplicationA", _ => new CommandFoo());
    }
}

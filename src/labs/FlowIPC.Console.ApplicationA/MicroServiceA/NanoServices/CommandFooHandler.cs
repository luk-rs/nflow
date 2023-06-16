namespace FlowIPC.Console.ApplicationA.MicroServiceA.NanoServices
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Flow.Reactive.Services;
    using FlowIPC.Console.ApplicationA.MicroServiceA.Commands;
    using FlowIPC.Console.ApplicationA.MicroServiceA.Streams.Public;

    public class CommandFooHandler : HandlerNano<CommandFoo>
    {
        public override IObservable<Unit> Connect() =>
            Handle
                .Do(_ => Console.WriteLine("Command Foo arrived!"))
                .UpdateAnd<CommandFoo, PersistedValue>(this, (_, stream) => stream.Value = 2)
                .Notify(this, _ => new SomethingHappened());
    }
}

namespace Flow.Reactive.IPC.Console.ApplicationA.MicroServiceA.NanoServices
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Flow.Reactive.IPC.Console.ApplicationA.MicroServiceA.Commands;
    using Flow.Reactive.IPC.Console.ApplicationA.MicroServiceA.Streams.Public;
    using Flow.Reactive.Services;

    public class CommandFooHandler : HandlerNano<CommandFoo>
    {
        public override IObservable<Unit> Connect() =>
            Handle
                .Do(_ => Console.WriteLine("Command Foo arrived!"))
                .UpdateAnd<CommandFoo, PersistedValue>(this, (_, stream) => stream.Value = 2)
                .Notify(this, _ => new SomethingHappened());
    }
}

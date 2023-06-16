namespace Flow.Nanos.Lab.MicroService.NanoServices
{
    using Flow.Nanos.Lab.MicroService.Commands;
    using Flow.Nanos.Lab.MicroService.Streams;
    using Flow.Reactive.Services;
    using System;
    using System.Reactive;

    public class CommandToUpdatedPersistedStreamHandler : HandlerNano<CommandToUpdatedPersistedStream>
    {
        public override IObservable<Unit> Connect() =>
            Handle.Update<CommandToUpdatedPersistedStream, MyPersistedStreamPayload>(this,
                (_, stream) => stream.Value++);
    }
}

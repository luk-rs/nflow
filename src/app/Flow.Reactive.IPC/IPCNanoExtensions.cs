namespace Flow.Reactive.IPC
{
    using Flow.Reactive.Extensions;
    using Flow.Reactive.IPC.IPCMicroService.Commands;
    using Flow.Reactive.Services;
    using Flow.Reactive.Streams.Ephemeral.Commands;
    using System;
    using System.Reactive;

    public static class IPCNanoExtensions
    {
        public static IObservable<Unit> SendIPCCommand<TInput>(this IObservable<TInput> value,
                                                               INano nano, 
                                                               string receiver,
                                                               Func<TInput, Command> selector)
            => value
                .SendCommand(nano, data => new IPCCommand(receiver, selector(data)));
    }
}

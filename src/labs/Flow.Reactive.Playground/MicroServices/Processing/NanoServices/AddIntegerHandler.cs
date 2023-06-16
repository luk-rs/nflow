namespace Flow.Reactive.Playground.MicroServices.Processing.NanoServices
{
    using Commands;
    using SharedKernel;
    using System;
    using Streams;
    using Flow.Reactive.Services;
    using System.Reactive;

    public class AddIntegerHandler : HandlerNano<AddInteger>
    {
        public override IObservable<Unit> Connect() =>
            Handle
            .Update<AddInteger, Integers>(this,
                                          (command, integers) =>
                                          integers.Add(new Integer { Value = command.Integer }));
    }
}
namespace Flow.Reactive.Playground.MicroServices.Reporting.NanoServices
{

    using Commands;
    using Streams;
    using System;
    using System.Linq;
    using Flow.Reactive.Services;
    using System.Reactive;

    public class ProcessIntegersHandler : HandlerNano<ProcessIntegers>
    {
        public override IObservable<Unit> Connect() =>
            Handle
            //.Log(this, command => $"{nameof(ProcessIntegersHandler)} handling {command.ShortFormat}")
            .Trace(command => $"{nameof(ProcessIntegersHandler)} handling {command.ShortFormat}")
            .Update<ProcessIntegers, Sum>(this,
                    (command, sum) => sum.Total = command.Integers.Sum(integer => integer.Value));
    }
}
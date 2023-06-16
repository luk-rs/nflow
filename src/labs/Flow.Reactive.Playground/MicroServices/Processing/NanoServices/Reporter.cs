namespace Flow.Reactive.Playground.MicroServices.Processing.NanoServices
{
    using Flow.Reactive.Playground.MicroServices.Reporting.Commands;
    using System;
    using Streams;
    using Flow.Reactive.Services;
    using System.Reactive;
    using System.Linq;

    public class Reporter : QueryNano<Integers>
    {
        public override IObservable<Unit> Connect() =>
            Query
            .SendCommand(this,
                         integers => new ProcessIntegers(integers.Items.ToList()));
    }
}
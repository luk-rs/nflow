namespace Flow.Castle.Windsor.Domain.Micro.NanoServices
{

    using System;
    using System.Reactive;
    using Commands;
    using Flow.Castle.Windsor.Domain.Micro2.Commands;
    using Flow.Castle.Windsor.Domain.Micro2.Streams;
    using Flow.Reactive.Services;
    using Reactive.Services.Nanos;
    using Streams;


    public class UpdateAHandler : CommandToQueryStreamNano<UpdateA, B>
    {

        protected override Func<UpdateA, Action<B>> Updater => update => b => b.Control = update.Value;

    }
}
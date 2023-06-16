namespace Flow.Castle.Windsor.Domain.Micro2.NanoServices
{

    using System;
    using System.Reactive;
    using Commands;
    using Flow.Castle.Windsor.Domain.Micro2.Commands;
    using Flow.Reactive.Services;
    using Reactive.Services.Nanos;
    using Streams;


    public class UpdateAHandler2 : CommandToQueryStreamNano<UpdateA2, B2>
    {

        protected override Func<UpdateA2, Action<B2>> Updater => update => b => b.Control = update.Value;

    }
}
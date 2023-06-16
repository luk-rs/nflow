namespace Flow.StructureMap.Console.Micro.Nanos
{

    using System;
    using Commands;
    using Reactive.Services.Nanos;
    using Streams;


    internal class UpdateAHandler : CommandToQueryStreamNano<UpdateA, B>
    {

        protected override Func<UpdateA, Action<B>> Updater => update => b => b.Control = update.Value;

    }

}
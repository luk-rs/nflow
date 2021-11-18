namespace nflow.core.Test.Nanos
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using nflow.core.Test.Services;
    using nflow.core.Test.Streams;

    internal class NanoZ : INano
    {

        public IObservable<Unit> Connect(IBus bus)
        {
            return bus
            .Instruction<CommandZ>()
            .Handle
            .Do(_ => bus.Whisper<StreamZ>().Gossip(payload => payload.Name = "Hello whisper"))
            .Select(_ii => Unit.Default);



        }

        private readonly Ii _ii;

        public NanoZ(Ii ii)
        {
            _ii = ii;
        }

    }
}
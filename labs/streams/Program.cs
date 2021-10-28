namespace streams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using streams.Core;
    using streams.Core.BusComponents;
    using streams.MicroServiceA.Commands;
    using streams.MicroServiceA.NanoServices;
    using streams.MicroServiceA.Streams;
    using streams.MicroServiceB.NanoServices;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var microABus = new Bus();
            microABus.LocalStreams.Add(typeof(Bar), new PersistedStream<Bar>());

            var microBBus = new Bus();

            var flow = new Flow(new[] { microABus, microBBus });

            flow.WireUp();

            var nanoA = new MyNanoServiceA();
            var nanoB = new MyNanoServiceB();
            var nanoA2 = new MyNanoServiceA2();
            var nanoB2 = new MyNanoServiceB2();

            nanoA.Connect(microABus).Subscribe();
            nanoA2.Connect(microABus).Subscribe();

            nanoB.Connect(microBBus).Subscribe();
            nanoB2.Connect(microBBus).Subscribe();

            //flow.SendCommand(new FooCommand());
            flow.SendCommand(new UpdateSomethingCommand());

            Console.WriteLine("Hello World!");
        }
    }

    public class Flow
    {
        private readonly List<IBus> _buses = new(); //Created after assemblies scanning

        public Flow(IEnumerable<IBus> buses) => _buses = new List<IBus>(buses);

        //1 - Scan assemblies
        //2 - Create one Bus per each MicroService
        //3 - Subscribe To Bus Streams / Commands Sent
        //4 - Invoke connect passing the respective bus

        public void WireUp()
        {
            _buses
                .Cast<ICommandBus>()
                .Select(bus => bus.CommandsSent)
                .Merge()
                .Do(command => _buses.Cast<ICommandBus>().ToList().ForEach(bus => bus.RouteCommand(command)))
                .Subscribe();

            _buses
                .Cast<IEventbus>()
                .Select(bus => bus.EventsSent)
                .Merge()
                .Do(@event => _buses.Cast<IEventbus>().ToList().ForEach(bus => bus.RouteEvent(@event)))
                .Subscribe();

            var allStreams = _buses
                .Cast<IStreamSource>()
                .SelectMany(streamSource => streamSource.LocalStreams)
                .Select(x => (x.Key, x.Value))
                .ToDictionary(x => x.Key, x => x.Value);

            _buses
                 .Cast<IStreamSource>()
                 .ToList()
                 .ForEach(bus => bus.AllStreams.AddRange(allStreams));
        }

        public void SendCommand(ICommand command) =>
            _buses
                .Cast<ICommandBus>()
                .ToList()
                .ForEach(bus => bus.RouteCommand(command));
    }
}


﻿namespace streams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using streams.MicroServiceA.Commands;
    using streams.MicroServiceA.NanoServices;
    using streams.MicroServiceA.Streams;
    using streams.MicroServiceB.NanoServices;
    using nflow.core.Flow;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var microABus = new Bus();
            microABus.AddOracle<Bar>();

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
        private readonly List<IMicroBus> _buses = new(); //Created after assemblies scanning

        public Flow(IEnumerable<IMicroBus> buses) => _buses = new List<IMicroBus>(buses);

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

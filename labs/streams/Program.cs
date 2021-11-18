namespace streams
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using nflow.core;
    using streams.Test.Commands;
    using streams.Test.Streams;

    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            // FactoryPatternInjection();

            await NonNanoConsumer();

        }

        private static void FactoryPatternInjection()
        {
            var services = new ServiceCollection()
                        .AddSingleton<Func<int, Constr>>(sp => @int => new Constr(@int, sp.GetRequiredService<IConstrInt>()))
                        .AddSingleton<IConstrInt, ConstrInt>();

            var provider = services
            .BuildServiceProvider();

            var c1 = provider
            .GetService<Func<int, Constr>>()
            (1);

            var c2 = provider
            .GetService<Func<int, Constr>>()
            (2);


            Console.WriteLine($"c1.Ref = {c1.Ref}\tc1.Name = {c2.ConstrInt.Name}\nc2.Ref = {c2.Ref}\tc2.Name = {c2.ConstrInt.Name}\n");
        }

        class Constr
        {

            public int Ref { get; }
            public IConstrInt ConstrInt { get; }

            public Constr(int @ref, IConstrInt constrInt)
            {
                Ref = @ref;
                ConstrInt = constrInt;
            }
        }

        interface IConstrInt
        {
            string Name => "Ola Inj";
        }

        class ConstrInt : IConstrInt { }


        private static async Task NonNanoConsumer()
        {
            var container = new ServiceCollection().AttachFlow().BuildServiceProvider();

            IFlow flow1 = container.GetRequiredService<IFlow>();


            var whisper = flow1.Bus.Whisper<StreamZ>().Listen.Do(z => Console.WriteLine(z.Name)).Take(50).ToTask();
            var instruction = flow1.Bus.Instruction<CommandZ>().Handle.Do(z => Console.WriteLine(z.Name)).Take(2).ToTask();


            var whispers = Observable
            .Range(0, 50)
            .Select(
                it => Observable
                .Return(Unit.Default)
                .Do(_ => flow1.Bus.Whisper<StreamZ>().Gossip(z => z.Name = $"Hello Whispers {it}"))
            )
            .Merge()
            .LastAsync()
            .ToTask();


            flow1.Bus.Instruction<CommandZ>().CommandTo(z => z.Name = "Hello Commands");
            flow1.Bus.Instruction<CommandZ>().CommandTo(z => z.Name += " Santos");


            await whispers;
            await instruction;
            await whisper;
        }
    }


}
// var microABus = new Bus();
// microABus.AddOracle<Bar>();

// var microBBus = new Bus();

// var flow = new Flow(new[] { microABus, microBBus });

// flow.WireUp();

// var nanoA = new MyNanoServiceA();
// var nanoB = new MyNanoServiceB();
// var nanoA2 = new MyNanoServiceA2();
// var nanoB2 = new MyNanoServiceB2();

// nanoA.Connect(microABus).Subscribe();
// nanoA2.Connect(microABus).Subscribe();

// nanoB.Connect(microBBus).Subscribe();
// nanoB2.Connect(microBBus).Subscribe();

// //flow.SendCommand(new FooCommand());
// flow.SendCommand(new UpdateSomethingCommand());

// Console.WriteLine("Hello World!");


//     public class Flow
//     {
//         private readonly List<IMicroBus> _buses = new(); //Created after assemblies scanning

//         public Flow(IEnumerable<IMicroBus> buses) => _buses = new List<IMicroBus>(buses);

//         //1 - Scan assemblies
//         //2 - Create one Bus per each MicroService
//         //3 - Subscribe To Bus Streams / Commands Sent
//         //4 - Invoke connect passing the respective bus

//         public void WireUp()
//         {
//             _buses
//                 .Cast<ICommandBus>()
//                 .Select(bus => bus.CommandsSent)
//                 .Merge()
//                 .Do(command => _buses.Cast<ICommandBus>().ToList().ForEach(bus => bus.RouteCommand(command)))
//                 .Subscribe();

//             _buses
//                 .Cast<IEventbus>()
//                 .Select(bus => bus.EventsSent)
//                 .Merge()
//                 .Do(@event => _buses.Cast<IEventbus>().ToList().ForEach(bus => bus.RouteEvent(@event)))
//                 .Subscribe();

//             var allStreams = _buses
//                 .Cast<IStreamSource>()
//                 .SelectMany(streamSource => streamSource.LocalStreams)
//                 .Select(x => (x.Key, x.Value))
//                 .ToDictionary(x => x.Key, x => x.Value);

//             _buses
//                  .Cast<IStreamSource>()
//                  .ToList()
//                  .ForEach(bus => bus.AllStreams.AddRange(allStreams));
//         }

//         public void SendCommand(ICommand command) =>
//             _buses
//                 .Cast<ICommandBus>()
//                 .ToList()
//                 .ForEach(bus => bus.RouteCommand(command));
//     }
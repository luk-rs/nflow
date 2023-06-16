namespace Flow.Nanos.Lab
{
    using Autofac;
    using Flow.Nanos.Lab.MicroService.Commands;
    using Flow.Reactive;
    using Flow.Reactive.Autofac;
    using System;

    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterFlowModule(new MicroRegistry("MicroService", typeof(Program).Assembly));

            using (var container = builder.Build())
            {
                Console.WriteLine("Press any key to instantiate Flow");
                Console.ReadKey();

                var flow = container.Resolve<IFlow>();

                Console.WriteLine("Press any key to send command to set persisted stream to 1");
                Console.ReadKey();

                flow
                    .Send(new CommandToUpdatedPersistedStream())
                    .Subscribe();

                Console.WriteLine("Press any key to send command to raise event with 1");
                Console.ReadKey();

                flow
                    .Send(new CommandToRaiseEvent(1))
                    .Subscribe();

                Console.WriteLine("Press any key to send command to raise event with 2");
                Console.ReadKey();

                flow
                   .Send(new CommandToRaiseEvent(2))
                   .Subscribe();

                Console.WriteLine("Press any key to send command to set persisted stream to 2");
                Console.ReadKey();

                flow
                    .Send(new CommandToUpdatedPersistedStream())
                    .Subscribe();

                Console.WriteLine("Press any key to send command to raise event with 1");
                Console.ReadKey();

                flow
                   .Send(new CommandToRaiseEvent(1))
                   .Subscribe();

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}

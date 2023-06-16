namespace Flow.Reactive.IPC.Console.ApplicationB
{
    using Flow.Reactive.Autofac;
    using Flow.Reactive.IPC.Console.ApplicationB.MicroServiceB.Commands;
    using Flow.Reactive.IPC.Console.ApplicationB.MicroServiceB.Streams.Public;
    using Flow.Reactive.IPC.IPCMicroService.Commands;
    using global::Autofac;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterFlowModule(new MicroRegistry("MicroServiceB", typeof(Program).Assembly),
                                       IPCConfigurator.Registry);

            using (var container = builder.Build())
            {
                var receiver = "Flow.Reactive.IPC.Console.ApplicationA";

                IPCConfigurator.SetCommunicationWith(receiver);

                var flow = container.Resolve<IFlow>();

                flow
                    .Send(new Subscribe(receiver, typeof(SomethingHappened).Name))
                    .Subscribe();

                flow
                  .Send(new Subscribe(receiver, typeof(PersistedValue).Name))
                  .Subscribe();

                Console.WriteLine("Press any key to start execution");
                Console.ReadKey();

                flow
                    .Send(new Start())
                    .Subscribe();

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}

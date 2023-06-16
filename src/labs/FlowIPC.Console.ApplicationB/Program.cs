namespace FlowIPC.Console.ApplicationB
{
    using Autofac;
    using Flow.Reactive;
    using Flow.Reactive.IPC;
    using Microsoft.Extensions.Logging;
    using System;
    using Flow.Reactive.Autofac;
    using FlowIPC.Console.ApplicationB.MicroServiceB.Commands;
    using Flow.Reactive.IPC.IPCMicroService.Commands;
    using FlowIPC.Console.ApplicationB.MicroServiceB.Streams.Public;

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterFlowModule(new MicroRegistry("MicroServiceB", typeof(Program).Assembly),
                                       IPCConfigurator.Registry);

            builder
                .RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            builder
                .RegisterType<LoggerFactory>()
                .As<ILoggerFactory>()
                .SingleInstance();

            using (var container = builder.Build())
            {
                var receiver = "FlowIPC.Console.ApplicationA";

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

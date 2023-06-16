namespace FlowIPC.Console.ApplicationA
{
    using Autofac;
    using Flow.Reactive;
    using Flow.Reactive.Autofac;
    using Flow.Reactive.IPC;
    using Microsoft.Extensions.Logging;
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterFlowModule(new MicroRegistry("MicroServiceA", typeof(Program).Assembly),
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
                IPCConfigurator.SetCommunicationWith("FlowIPC.Console.ApplicationB");

                var flow = container.Resolve<IFlow>();

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}

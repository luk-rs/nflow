namespace Flow.Reactive.IPC.Console.ApplicationA
{
    using Flow.Reactive.Autofac;
    using global::Autofac;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterFlowModule(new MicroRegistry("MicroServiceA", typeof(Program).Assembly),
                                       IPCConfigurator.Registry);

            using (var container = builder.Build())
            {
                IPCConfigurator.SetCommunicationWith("Flow.Reactive.IPC.Console.ApplicationB");

                var flow = container.Resolve<IFlow>();

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}

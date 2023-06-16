namespace Flow.Castle.Windsor.Console
{
    using Flow.Reactive;
    using Flow.Reactive.Castle.Windsor;
    using global::Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using global::Castle.Windsor;
    using Flow.Castle.Windsor.Domain.Micro.Commands;
    using global::Castle.MicroKernel.Registration;
    using Flow.Castle.Windsor.Domain.Services;
    using Flow.Castle.Windsor.Domain.Micro2.Commands;
    using Flow.Castle.Windsor.Domain.Micro.Streams;
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = new WindsorContainer();

            container.Install(new FlowInstaller(new[] { 
                new MicroRegistry("Micro", typeof(UpdateA).Assembly), 
                new MicroRegistry("Micro2", typeof(UpdateA2).Assembly) }
            , true));

            container.Register(
                    Component.For<IFoo>()
                    .ImplementedBy<Foo>()
                    .LifeStyle.Singleton);

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, allowEmptyCollections: true));

            IFlow flow = container.Resolve<IFlow>();

            //flow.Start();

            flow
                .Send(new UpdateA(2))
                .Subscribe();

            flow
                .Query<B>()
                .Subscribe(b => Console.WriteLine(b.Control));

            Console.ReadKey();
        }
    }
}

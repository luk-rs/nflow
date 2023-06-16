namespace Flow.Reactive.Tests.FlowTests.TestHelpers
{
    using Flow.Reactive.Autofac;
    using ContainerBuilder = global::Autofac.ContainerBuilder;
    using global::Autofac;
    using System.Linq;

    public static class FlowFactory
    {
        public static IFlow CreateFlow(params string [] microServices)
        {
            var builder = new ContainerBuilder();

            microServices
                .ToList()
                .ForEach(microService =>
                    builder.RegisterFlowModule(new MicroRegistry(microService, typeof(FlowFactory).Assembly)));

            var container = builder.Build();

            return container.Resolve<IFlow>();
        }

        public static IFlow CreateFlow(params (string Id, bool Transient)[] microServices)
        {
            var builder = new ContainerBuilder();

            microServices
                .ToList()
                .ForEach(microService =>
                    builder.RegisterFlowModule(new MicroRegistry(microService.Id, typeof(FlowFactory).Assembly, microService.Transient)));

            var container = builder.Build();

            return container.Resolve<IFlow>();
        }
    }
}

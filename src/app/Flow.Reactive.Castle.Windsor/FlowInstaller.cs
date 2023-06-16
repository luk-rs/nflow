namespace Flow.Reactive.Castle.Windsor
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Streams;
    using Flow.Reactive.Streams.Middleware;
    using global::Castle.MicroKernel.Registration;
    using global::Castle.MicroKernel.SubSystems.Configuration;
    using global::Castle.Windsor;
    using System.Collections.Generic;
    using System.Linq;

    public class FlowInstaller : IWindsorInstaller
    {
        private readonly List<MicroRegistry> _microRegistries;
        private readonly bool _deferredStart;
        private readonly bool _isMainFlowModule;

        public FlowInstaller(IEnumerable<MicroRegistry> microRegistries)
            : this(microRegistries, false)
        { }

        public FlowInstaller(IEnumerable<MicroRegistry> microRegistries, bool deferredStart)
            : this(microRegistries, deferredStart, true)
        { }

        public FlowInstaller(IEnumerable<MicroRegistry> microRegistries, bool deferredStart, bool isMainFlowModule)
        {
            _microRegistries = new List<MicroRegistry>(microRegistries);
            _deferredStart = deferredStart;
            _isMainFlowModule = isMainFlowModule;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (_isMainFlowModule)
            {
                container.Register(
                    Component.For<IFlow>()
                    .ImplementedBy<MasterFlow>()
                    .DynamicParameters((kernel, parameters) => parameters["deferredStart"] = _deferredStart)
                    .LifeStyle.Singleton);
            }

           _microRegistries
                 .ForEach(micro =>
                 {
                     RegisterStreams(container, micro);
                     RegisterNanos(container, micro);
                     RegisterMicros(container, micro);
                 });
        }

        private static void RegisterStreams(IWindsorContainer container, MicroRegistry micro) =>
            container.Register(
                Classes.FromAssemblyNamed(micro.Assembly.GetName().Name)
                .Where(type => typeof(IStream).IsAssignableFrom(type) && type.Namespace.Contains($"{micro.Namespace}.Streams"))
                .WithService.AllInterfaces()
                .Configure(component => component.LifestyleSingleton()));

        private static void RegisterNanos(IWindsorContainer container, MicroRegistry micro) =>
            container.Register(
                Classes.FromAssembly(micro.Assembly)
                .Where(type => typeof(INano).IsAssignableFrom(type) && type.Namespace.Contains($"{micro.Namespace}.NanoServices"))
                .WithService.AllInterfaces()
                .Configure(component => component.LifestyleSingleton()));

        private static void RegisterMicros(IWindsorContainer container, MicroRegistry micro) => 
            container.Register(
                  Component.For<IMicro>()
                  .Named(micro.Namespace)
                  .UsingFactoryMethod((kernel) =>
                  {
                      var streams = kernel
                        .ResolveAll<IStream>()
                        .Where(stream => stream.GetType().Namespace.Contains($"{micro.Namespace}.Streams"));

                      var nanos = kernel
                        .ResolveAll<INano>()
                        .Where(stream => stream.GetType().Namespace.Contains($"{micro.Namespace}.NanoServices"));

                      return new Micro(micro.Namespace, streams, nanos, Enumerable.Empty<IMiddleware>(), micro.Transient);
                  }));
    }
}

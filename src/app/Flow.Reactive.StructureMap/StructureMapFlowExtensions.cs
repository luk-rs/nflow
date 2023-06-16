namespace Flow.Reactive.StructureMap
{

    using System.Linq;
    using System.Reflection;
    using global::StructureMap;
    using Microsoft.Extensions.DependencyInjection;
    using Reactive;
    using Services;
    using Streams;
    using Streams.Middleware;

    public static class StructureMapFlowExtensions
    {

        public static Container AttachFlow(this Container container, params (string name, Assembly assembly)[] micros)
        {
            IServiceCollection services = new ServiceCollection();

            IServiceCollection scan_for<T>((string name, Assembly assembly) micro) where T : class
            {
                IServiceCollection scopedServices = new ServiceCollection();
                var types = micro.assembly
                                 .GetTypes()
                                 .Where(type => !type.IsAbstract)
                                 .Where(type => type.Namespace?.Contains(micro.name)??false)
                                 .Where(type => typeof(T).IsAssignableFrom(type))
                                 .Select(x => x)
                                 .ToList();

                types.ForEach(type => scopedServices.AddSingleton(typeof(T), type));
                return scopedServices;
            }

            var middlewareContainer = container.CreateChildContainer();
            var middlewares = scan_for<IMiddleware>(("Flow.Reactive", typeof(MasterFlow).Assembly));

            middlewareContainer.Populate(middlewares);

            micros.ToList()
                  .ForEach(micro =>
                   {
                       var streams = scan_for<IStream>(micro).BuildServiceProvider();
                       var nanosContainer = container.CreateChildContainer();
                       var nanos = scan_for<INano>(micro);
                       nanosContainer.Populate(nanos);

                       services.AddSingleton<IMicro>(_ => new Micro(micro.name,
                                                                    streams.GetServices(typeof(IStream)).Cast<IStream>(),
                                                                    nanosContainer.GetAllInstances<INano>(),
                                                                    middlewareContainer.GetAllInstances<IMiddleware>()));
                   });

            //TODO - Register Sagas
            //micros
            //    .Select(micro => micro.assembly)
            //    .Distinct()
            //    .SelectMany(assembly => assembly.GetTypes())
            //    .Where(type => !type.IsAbstract)
            //    .Where(type => type.Namespace != null)
            //    .Where(type => typeof(ISaga).IsAssignableFrom(type))
            //    .ToList()
            //    .ForEach(type => { }/*TODO*/);

            services.AddSingleton<IFlow, MasterFlow>();

            container.Populate(services);

            return container;
        }

    }

}
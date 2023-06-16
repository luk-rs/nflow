namespace Flow.Reactive.DependencyInjection
{

    using Microsoft.Extensions.DependencyInjection;
    using Reactive;
    using Services;
    using Streams;
    using Streams.Middleware;
    using System;
    using System.Linq;
    using System.Reactive.Subjects;
    using System.Reflection;


    public static class StructureMapFlowExtensions
    {

        public static void AttachFlow(this IServiceProvider container, SubjectBase<IServiceCollection> collections, params (string name, Assembly assembly)[] micros)
        {
            var services = new ServiceCollection();

            IServiceProvider scan_for<T>((string name, Assembly assembly) micro) where T : class
            {
                IServiceCollection scopedServices = new ServiceCollection();

                var types = micro.assembly
                                 .GetTypes()
                                 .Where(type => !type.IsAbstract)
                                 .Where(type => type.Namespace?.Contains(micro.name) ?? false)
                                 .Where(type => typeof(T).IsAssignableFrom(type))
                                 .Select(x => x)
                                 .ToList();

                types.ForEach(type =>
                {
                    scopedServices.AddSingleton(type,
                                                sp =>
                                                {
                                                    ConstructorInfo constructor = type.GetConstructors().Single();
                                                    var dependencies = constructor.GetParameters()
                                                                                  .Select(parameter => parameter.ParameterType)
                                                                                  .Select(container.GetService)
                                                                                  .ToArray();
                                                    var instance = Activator.CreateInstance(type, dependencies);
                                                    return instance;
                                                });
                    scopedServices.AddSingleton(_ => container.GetService(type) as T);
                });

                container
                       .GetServices<IMiddleware>()
                       .ToList()
                       .ForEach(customMiddleware => scopedServices.Add(new ServiceDescriptor(typeof(IMiddleware), customMiddleware)));

                collections.OnNext(scopedServices);
                return scopedServices.BuildServiceProvider();
            }

            IServiceProvider middleware = scan_for<IMiddleware>(("Flow.Reactive", typeof(MasterFlow).Assembly));

            micros.ToList()
                  .ForEach(micro =>
                   {
                       IServiceProvider streams = scan_for<IStream>(micro);
                       IServiceProvider nanos = scan_for<INano>(micro);

                       services.AddSingleton<IMicro>(sp => new Micro(micro.name,
                                                                     streams.GetServices(typeof(IStream)).Cast<IStream>(),
                                                                     nanos.GetServices<INano>(),
                                                                     middleware.GetServices<IMiddleware>()));
                   });

            services.AddSingleton<IFlow, MasterFlow>();
            collections.OnNext(services);
            collections.OnCompleted();
        }

    }

}
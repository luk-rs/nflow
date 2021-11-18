namespace nflow.core
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;


    internal interface IServicesResolver
    {
        TService Resolve<TService>() where TService : class;

        void AttachTo(IServiceCollection services);
    }


    internal class ServicesResolver : IServicesResolver
    {

        TService IServicesResolver.Resolve<TService>() => _provider.GetRequiredService<TService>();
        void IServicesResolver.AttachTo(IServiceCollection target)
        {
            Func<IServiceProvider, object> resolve(Type type) => _ => _provider.GetRequiredService(type);

            _services.ToList()
                    .ForEach(descriptor =>
                    {
                        var serviceType = descriptor.ServiceType;

                        Action bridgeWithLifetime = descriptor.Lifetime switch
                        {
                            ServiceLifetime.Singleton => () => target.AddSingleton(serviceType, resolve(serviceType)),
                            ServiceLifetime.Transient => () => target.AddTransient(serviceType, resolve(serviceType)),
                            ServiceLifetime.Scoped => () => target.AddScoped(serviceType, resolve(serviceType)),
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        bridgeWithLifetime();
                    });
        }

        public ServicesResolver(IEnumerable<Registry> registries)
        {
            _services = registries.Cast<IServiceCollection>().Aggregate((prev, cur) => prev.Add(cur));
            _provider = _services.BuildServiceProvider();
        }

        private readonly ServiceProvider _provider;
        private IServiceCollection _services;
    }
}
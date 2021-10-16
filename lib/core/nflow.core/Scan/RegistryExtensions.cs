using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace nflow.core.Scan
{
    public static class RegistryExtensions
    {
        public static RegistryScan AutoScan(this IServiceCollection services, Assembly origin =default)
        {
            services.Scan(
                scanner => scanner
                    .FromAssemblyDependencies((origin ?? Assembly.GetEntryAssembly())!)
                    .AddClasses(classes => classes.Where(type => type.IsSubclassOf(typeof(Registry))))
                    .As<Registry>());
            
            var serviceProvider = services.BuildServiceProvider();
            var servicesDefinition =serviceProvider.GetServices<Registry>();

            return new RegistryScan(servicesDefinition);
        }
    }
}
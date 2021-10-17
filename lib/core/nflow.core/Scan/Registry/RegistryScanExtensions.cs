using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using nflow.core.Abstractions;

namespace nflow.core.Scan
{
    public static class RegistryScanExtensions
    {
        public static RegistryProvider ScanRegistries(this IServiceCollection services, Assembly origin =default)
        {
            services.Scan(
                scanner => scanner
                    .FromAssemblyDependencies((origin ?? Assembly.GetEntryAssembly())!)
                    .AddClasses(classes => classes.Where(type => type.IsSubclassOf(typeof(Registry))))
                    .As<Registry>());
            
            using var serviceProvider = services.BuildServiceProvider();
            var servicesDefinition =serviceProvider.GetServices<Registry>();

            return new RegistryProvider(servicesDefinition);
        }
    }
}
namespace nflow.core
{
    using System;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;


    public static class FlowInstallationExtensions
    {

        public static IServiceCollection AttachFlow(this IServiceCollection services, Assembly origin = default)
        {
            var registry = new BootstrapRegistry();

            var assembly = new OriginAssembly(origin == default ? Assembly.GetEntryAssembly() : origin);
            registry.AddSingleton<OriginAssembly>(_ => assembly);

            registry.ScanRegistries(assembly);

            var bootstrap = registry.BuildServiceProvider();

            services.AddSingleton(_ => bootstrap.GetRequiredService<IFlow>());

            return services;
        }

        internal static BootstrapRegistry ScanRegistries(this BootstrapRegistry registry, OriginAssembly assembly)
        {

            static bool valid_registries(Type type) => type.IsSubclassOf(typeof(Registry)) && !type.IsAssignableFrom(typeof(BootstrapRegistry));

            registry.Scan(
                scanner => scanner
                    .FromAssemblyDependencies(assembly.Instance)
                    .AddClasses(classes => classes.Where(type => valid_registries(type)))
                    .As<Registry>());

            return registry;

        }


    }
}
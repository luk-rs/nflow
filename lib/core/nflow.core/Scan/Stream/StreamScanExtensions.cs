using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using nflow.core.Abstractions;

namespace nflow.core.Scan.Stream
{
    public static class StreamScanExtensions
    {
        public static StreamProvider ScanStreams(this IServiceCollection services, Assembly origin = default)
        {
            services.Scan(
                scanner => scanner
                    .FromAssemblyDependencies((origin ?? Assembly.GetEntryAssembly())!)
                    .AddClasses(classes => classes.AssignableTo<IStream>())
                    .As<IStream>()
                    .WithSingletonLifetime());

            return new StreamProvider(services);
        }
    }
}
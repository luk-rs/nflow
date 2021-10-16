using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace nflow.core.Scan
{
    public sealed class RegistryScan
    {
        private readonly ServiceProvider _provider;

        public RegistryScan(IEnumerable<Registry> registries)
        {
            var services = registries
                .Cast<IServiceCollection>()
                .Aggregate((prev, cur) => prev.Add(cur));

            _provider = services.BuildServiceProvider();
        }

        public T Service<T>() => _provider.GetService<T>();
        public IEnumerable<T> AllServices<T>() => _provider.GetServices<T>();

        public T RequiredService<T>() => _provider.GetRequiredService<T>();
    }
}
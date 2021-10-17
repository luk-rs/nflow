using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using nflow.core.Abstractions;

namespace nflow.core.Scan
{
    public sealed class RegistryProvider
    {
        private readonly IServiceProvider _provider;

        public RegistryProvider(IEnumerable<Registry> registries)
        {
            var services = registries
                .Cast<IServiceCollection>()
                .Aggregate((prev, cur) => prev.Add(cur));

            _provider = services.BuildServiceProvider();
        }

        public T Service<T>()
        {
            return _provider.GetService<T>();
        }

        public IEnumerable<T> AllServices<T>()
        {
            return _provider.GetServices<T>();
        }

        public T RequiredService<T>()
        {
            return _provider.GetRequiredService<T>();
        }
    }
}
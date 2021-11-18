namespace nflow.core
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;

    internal interface INanosResolver
    {
        IEnumerable<INano> All { get; }
        IEnumerable<INano> Of(string @namespace);

    }

    internal class NanosResolver : INanosResolver
    {

        IEnumerable<INano> INanosResolver.All => _provider.GetServices<INano>();
        IEnumerable<INano> INanosResolver.Of(string @namespace) => ((INanosResolver)this).All.Where(nano => nano.GetType().Namespace.StartsWith(@namespace));

        public NanosResolver(IServicesResolver services, OriginAssembly assembly)
        {
            var nanos = new ServiceCollection();

            services.AttachTo(nanos);

            nanos.Scan(scanner => scanner.FromAssemblyDependencies(assembly.Instance)
                                      .AddClasses(classes => classes.AssignableTo<INano>())
                                      .As<INano>()
                                      .WithSingletonLifetime());

            _provider = nanos.BuildServiceProvider();
        }

        private readonly IServiceProvider _provider;

    }
}
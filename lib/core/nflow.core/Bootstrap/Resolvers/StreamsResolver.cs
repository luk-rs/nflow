namespace nflow.core
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;


    internal interface IStreamsResolver
    {

        IEnumerable<IStream> All { get; }
        IEnumerable<IStream> Public { get; }
        IEnumerable<IStream> Private { get; }

        IEnumerable<IStream> Of(string @namespace);
    }

    internal class StreamsResolver : IStreamsResolver
    {

        IEnumerable<IStream> IStreamsResolver.All => _streamsProvider.GetServices<IStream>();
        IEnumerable<IStream> IStreamsResolver.Public => ((IStreamsResolver)this).All.Where(stream => stream.IsPublic);
        IEnumerable<IStream> IStreamsResolver.Private => ((IStreamsResolver)this).All.Where(stream => !stream.IsPublic);

        IEnumerable<IStream> IStreamsResolver.Of(string @namespace) => ((IStreamsResolver)this).All.Where(stream => stream.GetType().Namespace.StartsWith(@namespace));

        public StreamsResolver(IEnumerable<Registry> registries, IServicesResolver servicesResolver, OriginAssembly assembly)
        {
            var streams = new ServiceCollection();

            //TODO create structure for oracles and whispers here

            streams.Scan(
                    scanner => scanner.FromAssemblyDependencies(assembly.Instance)
                                      .AddClasses(classes => classes.AssignableTo<IStream>())
                                      .As<IStream>()
                                      .WithSingletonLifetime());

            _streamsProvider = streams.BuildServiceProvider();


        }

        private readonly ServiceProvider _streamsProvider;
    }
}
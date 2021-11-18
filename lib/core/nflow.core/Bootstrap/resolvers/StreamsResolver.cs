
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}
namespace nflow.core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;

    internal interface IStreamsResolver
    {
        MicroStreams Types { get; }
        MicroHooks Hooks { get; }

    }

    internal class StreamsResolver : IStreamsResolver
    {

        MicroStreams IStreamsResolver.Types => _microStreams;
        MicroHooks IStreamsResolver.Hooks => _microHooks;


        public StreamsResolver(IEnumerable<Registry> registries, IServicesResolver servicesResolver, OriginAssembly assembly)
        {
            var (collection, provider) = ScanStreams(assembly);
            _microStreams = new MicroStreams(provider.GetServices<IStream>().ToArray());


            var hooks = new ServiceCollection();

            BootstrapInstructions(collection, hooks);
            BootstrapOracles(collection, hooks);
            BootstrapWhispers(collection, hooks);

            var hooksProvider = hooks.BuildServiceProvider();


            IHook[] filter<T>() => hooksProvider
                        .GetServices<IHook>()
                        .Where(hook => hook.Holding(typeof(T)))
                        .ToArray();

            var oracles = filter<IOracle>();
            var whispers = filter<IWhisper>();
            var instructions = filter<ICommand>();

            _microHooks = new MicroHooks(oracles, whispers, instructions);

        }
        private readonly MicroStreams _microStreams;
        private readonly MicroHooks _microHooks;

        private IStreamsResolver Self => this as IStreamsResolver;

        private (ServiceCollection collection, ServiceProvider provider) ScanStreams(OriginAssembly assembly)
        {
            var streams = new ServiceCollection();

            streams.Scan(
                    scanner => scanner.FromAssemblyDependencies(assembly.Instance)
                                      .AddClasses(classes => classes.AssignableTo<IStream>())
                                      .As<IStream>()
                                      .WithSingletonLifetime());

            var streamsProvider = streams.BuildServiceProvider();

            return (streams, streamsProvider);
        }

        private static void BootstrapOracles(ServiceCollection streams, ServiceCollection hooks)
        => Bootstrap<IOracle, IOracleBus<IOracle>>(streams, hooks, typeof(Oracle<>));

        private static void BootstrapInstructions(ServiceCollection streams, ServiceCollection hooks)
        => Bootstrap<ICommand, IInstructionsBus<ICommand>>(streams, hooks, typeof(Instruction<>));
        private static void BootstrapWhispers(ServiceCollection streams, ServiceCollection hooks)
        => Bootstrap<IWhisper, IWhispersBus<IWhisper>>(streams, hooks, typeof(Whisper<>));

        private static void Bootstrap<TStream, TGenericBus>(ServiceCollection streams, ServiceCollection hooks, Type container)
        {
            var instances = streams
                        .Where(descriptor =>
                        {
                            var generic = typeof(TStream);
                            var stream = descriptor.ImplementationType;
                            return !stream.IsAbstract && generic.IsAssignableFrom(descriptor.ImplementationType);
                        })
                        .Select(descriptor => (descriptor, instance: CreateGenericHook(container, descriptor.ImplementationType)));

            instances
            .ToList()
            .ForEach(instance =>
            {
                hooks.AddSingleton(typeof(IHook), _ => instance.instance);
                hooks.AddSingleton(typeof(TGenericBus), _ => instance.instance);
                var type = container.MakeGenericType(instance.descriptor.ImplementationType);
                hooks.AddSingleton(type, _streamsProvider => instance.instance);
            });

        }


        private static IHook CreateGenericHook(Type @class, Type generic)
        {
            var activation = @class.MakeGenericType(generic);
            var obj = Activator.CreateInstance(activation);
            return obj as IHook;
        }

    }
}
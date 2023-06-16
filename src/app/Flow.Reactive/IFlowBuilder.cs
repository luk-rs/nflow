namespace Flow.Reactive
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IFlowBuilder
    {
        IFlowBuilder WithMicro(MicroRegistry microRegistry);

        IFlowBuilder WithMicros(params MicroRegistry[] microRegistries);

        IFlow Build();

    }

    public class FlowBuilder : IFlowBuilder
    {
        private readonly List<MicroRegistry> _microRegistries = new List<MicroRegistry>();

        public static IFlowBuilder Create() => new FlowBuilder();

        public IFlowBuilder WithMicro(MicroRegistry microRegistry)
        {
            _microRegistries.Add(microRegistry);

            return this;
        }

        public IFlow Build() => null;

        public IFlowBuilder WithMicros(params MicroRegistry[] microRegistries)
        {
            _microRegistries.AddRange(microRegistries);

            return this;
        }
    }

    public class MicroRegistry
    {
        public MicroRegistry(string @namespace, Assembly assembly) 
            : this(@namespace, assembly, new ServiceCollection(), transient: false)
        {}

        public MicroRegistry(string @namespace, Assembly assembly, bool transient)
           : this(@namespace, assembly, new ServiceCollection(), transient)
        { }

        public MicroRegistry(string @namespace, Assembly assembly, ServiceCollection serviceCollection)
           : this(@namespace, assembly, serviceCollection, transient: false)
        { }

        public MicroRegistry(string @namespace, Assembly assembly, ServiceCollection serviceCollection, bool transient)
        {
            Namespace = @namespace;
            Assembly = assembly;
            ServiceCollection = serviceCollection;
            Transient = transient;
        }

        public string Namespace { get; }

        public Assembly Assembly { get; }

        public ServiceCollection ServiceCollection { get; }

        public bool Transient { get; }
    }

    public class FakeMicroRegistry : MicroRegistry
    {
        public FakeMicroRegistry(string @namespace, Assembly assembly, string realMicroServiceNamespace, Assembly realMicroServiceAssembly) 
            : base(@namespace, assembly)
        {
            RealMicroServiceNamespace = realMicroServiceNamespace;
            RealMicroServiceAssembly = realMicroServiceAssembly;
        }

        public string RealMicroServiceNamespace { get; }

        public Assembly RealMicroServiceAssembly { get; }
    }
}

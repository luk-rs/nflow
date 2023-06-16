namespace Flow.Reactive.Autofac
{

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Flow.Reactive.Sagas;
    using global::Autofac;
    using global::Autofac.Extensions.DependencyInjection;
    using Reactive;
    using Services;
    using Streams;
    using Streams.Middleware;
    using Module = global::Autofac.Module;

    public class FlowModule : Module
    {
        private readonly List<MicroRegistry> _microRegistries;
        private readonly bool _deferredStart;
        private readonly bool _isMainFlowModule;

        public FlowModule(IEnumerable<MicroRegistry> microRegistries) 
            : this(microRegistries, false)
        { }

        public FlowModule(IEnumerable<MicroRegistry> microRegistries, bool deferredStart)
         : this(microRegistries, deferredStart, true)
        { }

        public FlowModule(IEnumerable<MicroRegistry> microRegistries, bool deferredStart, bool isMainFlowModule)
        {
            _microRegistries = new List<MicroRegistry>(microRegistries);
            _deferredStart = deferredStart;
            _isMainFlowModule = isMainFlowModule;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_isMainFlowModule)
            {
                builder
                    .RegisterType<MasterFlow>()
                    .As<IFlow>()
                    .WithParameter("deferredStart", _deferredStart)
                    .SingleInstance();

                builder
                    .RegisterAssemblyTypes(typeof(MasterFlow).Assembly)
                    .Where(type => typeof(IMiddleware).IsAssignableFrom(type))
                    .As<IMiddleware>();
            }

            _microRegistries
                   .ForEach(micro =>
                    {
                         switch(micro)
                         {
                            case FakeMicroRegistry fakeMicro:
                                RegisterFakeMicroService(fakeMicro);
                                break;
                            default:
                                RegisterRealMicroService(micro);
                                break;
                         }
                    });

            RegisterSagas(builder, _microRegistries
                                        .Select(micro => micro.Assembly)
                                        .Distinct());

            void RegisterRealMicroService(MicroRegistry micro)
            {
                RegisterMicros(builder, micro);
                RegisterNanos(builder, micro);

                builder
                    .RegisterAssemblyTypes(micro.Assembly)
                    .Where(type => typeof(IStream).IsAssignableFrom(type) && type.Namespace.Contains($"{micro.Namespace}.Streams"))
                    .Keyed<IStream>($"{micro.Namespace}.Stream");
            }

            void RegisterFakeMicroService(FakeMicroRegistry micro)
            {
                RegisterMicros(builder, micro);
                RegisterNanos(builder, micro);

                builder
                   .RegisterAssemblyTypes(micro.RealMicroServiceAssembly)
                   .Where(type => typeof(IStream).IsAssignableFrom(type) && type.Namespace.Contains($"{micro.RealMicroServiceNamespace}.Streams"))
                   .Keyed<IStream>($"{micro.Namespace}.Stream");
            }
        }

        private static void RegisterMicros(ContainerBuilder builder, MicroRegistry micro)
        {
            builder
                .Register(c =>
                {
                    var microStreams = c.ResolveKeyed<IEnumerable<IStream>>($"{micro.Namespace}.Stream");
                    var microNanos = c.ResolveKeyed<IEnumerable<INano>>($"{micro.Namespace}.Nano");
                    var middleware = c.Resolve<IEnumerable<IMiddleware>>();

                    return new Micro(micro.Namespace, microStreams, microNanos, middleware, micro.Transient);
                })
                .As<IMicro>();

            builder.Populate(micro.ServiceCollection);
        }

        private static void RegisterNanos(ContainerBuilder builder, MicroRegistry micro)
        {
            builder
                .RegisterAssemblyTypes(micro.Assembly)
                .Where(type => typeof(INano).IsAssignableFrom(type) && type.Namespace.Contains($"{micro.Namespace}.NanoServices"))
                .Keyed<INano>($"{micro.Namespace}.Nano");
        }

        private static void RegisterSagas(ContainerBuilder builder, IEnumerable<Assembly> assemblies)
        {
            assemblies
                .ToList()
                .ForEach(assembly =>
                    builder
                        .RegisterAssemblyTypes(assembly)
                        .Where(type => type.Namespace != null && typeof(ISaga).IsAssignableFrom(type) && !type.IsAbstract)
                        .AsSelf()
                        .AutoActivate()
                );
        }
    }
}
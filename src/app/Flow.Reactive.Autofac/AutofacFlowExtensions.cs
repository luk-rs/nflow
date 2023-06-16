namespace Flow.Reactive.Autofac
{

    using global::Autofac;
    using Reactive;


    public static class AutofacFlowExtensions
    {
        public static void RegisterFlowModule(this ContainerBuilder builder, params MicroRegistry[] microRegistries) =>
            RegisterFlowModule(builder, false, microRegistries);

        public static void RegisterFlowModule(this ContainerBuilder builder, bool deferredStart, params MicroRegistry[] microRegistries)
        {
            var flowModule = new FlowModule(microRegistries, deferredStart);

            builder.RegisterModule(flowModule);
        }

        public static void RegisterPluginFlowModule(this ContainerBuilder builder, params MicroRegistry[] microRegistries)
        {
            var flowModule = new FlowModule(microRegistries, false, false);

            builder.RegisterModule(flowModule);
        }
    }
}

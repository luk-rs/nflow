namespace nflow.core
{
    using Microsoft.Extensions.DependencyInjection;

    internal class BootstrapRegistry : Registry
    {
        public BootstrapRegistry()
        {
            this.AddSingleton<IFlow, Flow>();

            this.AddSingleton<IStreamsResolver, StreamsResolver>();
            this.AddSingleton<IServicesResolver, ServicesResolver>();
            this.AddSingleton<INanosResolver, NanosResolver>();
            this.AddSingleton<IMicrosResolver, MicrosResolver>();

            this.AddSingleton<FlowBus>();
        }
    }
}
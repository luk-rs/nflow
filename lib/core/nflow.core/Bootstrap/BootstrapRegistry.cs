namespace nflow.core
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    internal class BootstrapRegistry : Registry
    {
        public BootstrapRegistry()
        {

            // AddFactories();

            this.AddSingleton<IFlow, Flow>();

            this.AddSingleton<IStreamsResolver, StreamsResolver>();
            this.AddSingleton<IServicesResolver, ServicesResolver>();
            this.AddSingleton<INanosResolver, NanosResolver>();
            this.AddSingleton<IMicrosResolver, MicrosResolver>();

            this.AddSingleton<IEnumerable<IMicro>>(sp => sp.GetRequiredService<IMicrosResolver>().All);
            this.AddSingleton<IEnumerable<INano>>(sp => sp.GetRequiredService<INanosResolver>().All);



            this.AddSingleton<FlowBus>();
        }

        // private void AddFactories() => this.AddSingleton<Func<Registry, IMicro>>(sp);
    }
}
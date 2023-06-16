namespace Flow.Wpf
{

    using System.Reflection;
    using System.Windows;
    using Caliburn;
    using Host;
    using Microsoft.Extensions.DependencyInjection;


    public class CaliburnHost<T> : Host
    {

        private Application App { get; set; }
        private ServiceProviderBootstrapper<T> Bootstrapper { get; set; }

        public CaliburnHost(Application app, params (string nmspc, Assembly assembly)[] micro)
                : base(micro)
            => Run(sp =>
            {
                App = app;
                Bootstrapper = sp.GetService<ServiceProviderBootstrapper<T>>();
                App.Resources
                   .MergedDictionaries
                   .Add(new ResourceDictionary
                    {
                            {"bootstrapper", Bootstrapper}
                    });
            });

        public void Shutdown(int exitCode) => App.Shutdown(exitCode);

    }

}
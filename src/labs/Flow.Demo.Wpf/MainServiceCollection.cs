namespace Flow.Demo.Wpf
{
    using Flow.Demo.Wpf.ViewModels;
    using Microsoft.Extensions.DependencyInjection;
    using Flow.Wpf.Caliburn;

    public class MainServiceCollection : ServiceCollection
    {

        public MainServiceCollection() => this
                                         .AddSingleton<ServiceProviderBootstrapper<MainViewModel>>()
                                         .AddSingleton<MainViewModel>();

    }
}
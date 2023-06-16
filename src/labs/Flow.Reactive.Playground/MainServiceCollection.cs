namespace Flow.Reactive.Playground
{
    using Flow.Reactive.Logging;
    using Flow.Reactive.Streams.Middleware;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Wpf.Caliburn;


    public class MainServiceCollection : ServiceCollection
    {
        public MainServiceCollection()
        {
            this
             .AddSingleton<ServiceProviderBootstrapper<MainViewModel>>()
             .AddSingleton<MainViewModel>()
             .AddSingleton<IMiddleware, PlaygroundCustomMiddleware>()
             .AddSingleton<IMiddleware>(sp => new CommandLoggerMiddleware("Playground Command"));
        }
    }
}
using Caliburn.Micro;

namespace Flow.Wpf
{

    using Microsoft.Extensions.DependencyInjection;


    public class CaliburnServiceCollection : ServiceCollection
    {

        public CaliburnServiceCollection() => this
                                             .AddTransient<IWindowManager, Caliburn.WindowManager>()
                                             .AddSingleton<IEventAggregator, EventAggregator>();

    }

}
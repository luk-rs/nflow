namespace Flow.Host
{

    using Microsoft.Extensions.DependencyInjection;


    public class HostServiceCollection : ServiceCollection
    {

        public HostServiceCollection()
        {
            this.AddTransient<CommandLineParser>();

            //Environment.GetEnvironmentVariable("env") switch
            //{
            //        "teste" => this.AddSingleton<ISchedulerProvider, TestSchedulerProvider>(),
            //        _ => this.AddSingleton<ISchedulerProvider, SchedulerProvider>()
            //};
        }

    }

}
namespace Flow.Host
{

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using CommandLine;
    using Microsoft.Extensions.DependencyInjection;
    using Reactive.DependencyInjection;
    using Flow.Reactive.DependencyInjection;


    public class Host : IDisposable
    {

        public int Run(Func<IServiceProvider, int> func)
            => ParseArgumentsAndRun<CommandLineParser.Undefined>(new string[0], (sp, _) => func(sp), default);

        public void Run(Action<IServiceProvider> action)
            => ParseArgumentsAndRun<CommandLineParser.Undefined>(new string[0],
                                                                 (sp, _) =>
                                                                 {
                                                                     action(sp);
                                                                     return 0;
                                                                 },
                                                                 default);

        public int Run<Options>(string[] args, Func<IServiceProvider, Options, int> func, Func<IServiceProvider, IEnumerable<Error>, string[], int> errorHandle)
                where Options : class => ParseArgumentsAndRun(args, func, errorHandle);

        public void Run<Options>(string[] args, Action<IServiceProvider, Options> action, Action<IServiceProvider, IEnumerable<Error>, string[]> errorHandle)
                where Options : class => ParseArgumentsAndRun<Options>(args,
                                                                       (sp, opts) =>
                                                                       {
                                                                           action(sp, opts);
                                                                           return 0;
                                                                       },
                                                                       (sp, errors, args) =>
                                                                       {
                                                                           errorHandle(sp, errors, args);
                                                                           return -1000;
                                                                       });

        public Host(params (string nmspc, Assembly assembly)[] micros)
        {
            AnchorConsole.Attach();
            container = new Container(micros);
        }

        private IContainer container { get; }

        private int ParseArgumentsAndRun<Options>(string[] args, Func<IServiceProvider, Options, int> run, Func<IServiceProvider, IEnumerable<Error>, string[], int> errorHandle)
                where Options : class
            => container
              .GetService<CommandLineParser>()
              .Parse(args, run, errorHandle);

        public virtual void Dispose() => AnchorConsole.Free();

    }


    class TestHost
    {

        public TestHost()
        {
            Environment.SetEnvironmentVariable("env", "teste");
        }

    }

}
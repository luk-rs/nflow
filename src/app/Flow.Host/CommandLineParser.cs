namespace Flow.Host
{

    using System;
    using System.Collections.Generic;
    using CommandLine;
    using Reactive.DependencyInjection;


    internal class CommandLineParser
    {

        public int Parse<Options>(string[] args, Func<IServiceProvider, Options, int> run, Func<IServiceProvider, IEnumerable<Error>, string[], int> errorHandle)
                where Options : class => new Parser().ParseArguments<Options>(args)
                                                     .MapResult(opts => run(_container, opts),
                                                                errors => errorHandle(_container, errors, args));


        #region construction

        private readonly IContainer _container;

        public CommandLineParser(IContainer container)
        {
            _container = container;
        }

        #endregion


        public class Undefined { }

    }

}
namespace Flow.StructureMap.Console
{

    using System;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Reflection;
    using System.Threading.Tasks;
    using global::StructureMap;
    using Micro.Commands;
    using Micro.Streams;
    using Reactive;
    using Reactive.StructureMap;
    using Services;


    class Program
    {

        static async Task Main(string[] args)
        {
            //emulating application existing container
            var container = new Container(cfg =>
                    {
                        cfg.For<IFoo>().Use<Foo>();
                    })
                    ////filters from assemblies which contain micros
                   .AttachFlow(("Micro", Assembly.GetEntryAssembly()),
                               ("Services", Assembly.GetEntryAssembly()));

            var flow = container.GetInstance<IFlow>();
            var foo = container.GetInstance<IFoo>();

            var control = 69;

            var task = flow
                      .Query<B>()
                      .ObserveOn(Scheduler.Default)
                      .Do(b => Console.WriteLine(b.Control == control
                                                         ? $"whooohoo control {b.Control} arrived"
                                                         : $"missing control on {b.Control}"))
                      .TakeUntil(b => b.Control == control)
                      .ToTask();

            _ = Task.Run(() => Observable.Range(0, 100)
                                         .Select(_ => flow.Send(new UpdateA(foo.Next)))
                                         .Concat()
                                         .LastAsync()
                                         .Select(_ => flow.Send(new UpdateA(control)))
                                         .Switch()
                                         .Subscribe());
            await task;
        }

    }

}
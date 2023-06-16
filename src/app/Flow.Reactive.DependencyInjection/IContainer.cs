namespace Flow.Reactive.DependencyInjection
{

    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Reactive.Threading.Tasks;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Reactive;


    public interface IContainer : IServiceProvider { }


    internal class Container : IContainer
    {

        public (string nmspc, Assembly assembly)[] Micros { get; }

        public object Resolve(Type serviceType) => Resolve(provider => provider.GetService(serviceType));
        public T Resolve<T>() where T : class => Resolve(provider => provider.GetService<T>()) as T;
        public object GetService(Type serviceType) => Resolve(provider => provider.GetService(serviceType));


        #region construction

        public Container(params (string nmspc, Assembly assembly)[] micros)
        {
            Micros = micros;

            Registry.AddTransient<IContainer>(_ => this);
            Provider.OnNext(Registry.BuildServiceProvider());

            Changes.Do(mergedCollection => Provider.OnNext(mergedCollection.BuildServiceProvider()))
                   .Subscribe();

            this.AttachFlow(FlowServices, Micros);

            _ = Resolve<IFlow>();
        }

        #endregion


        private IServiceCollection Registry { get; } = Workspace.TypesFor<IServiceCollection>()
                                                                .Select(Activator.CreateInstance)
                                                                .Cast<IServiceCollection>()
                                                                .Aggregate((acc, cur) => new ServiceCollection {acc, cur});

        SubjectBase<IServiceCollection> FlowServices { get; } = new BehaviorSubject<IServiceCollection>(default);

        private IObservable<IServiceCollection> Changes => FlowServices
                                                          .Where(collection => collection != default)
                                                          .Do(services => services.AddTransient<IContainer>(_ => this))
                                                          .Select(x => x)
                                                          .Scan((prev, cur) => new ServiceCollection {prev, cur})
                                                          .Select(change => new ServiceCollection {Registry, change})
                                                          .LastAsync();

        BehaviorSubject<IServiceProvider> Provider { get; } = new BehaviorSubject<IServiceProvider>(null);

        object Resolve(Func<IServiceProvider, object> resolve)
            => Provider
              .Select(resolve)
              .Take(1)
              .ToTask()
              .Result;

    }

}
using Caliburn.Micro;

namespace Flow.Wpf.Caliburn
{

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;
    using Reactive.DependencyInjection;
    using IContainer = Reactive.DependencyInjection.IContainer;


    public class ServiceProviderBootstrapper<T> : BootstrapperBase
    {

        protected override void OnStartup(object sender, StartupEventArgs e) => DisplayRootViewFor<T>();

        protected override object GetInstance(Type service, string key)
        {
            object instance = _container.GetService(service);
            return instance;
        }

        protected override IEnumerable<object> GetAllInstances(Type service) => _container.GetServices(service);

        protected override IEnumerable<Assembly> SelectAssemblies() => Workspace.AssembliesWithTypes(type => type.Name.EndsWith("View"));


        #region construction

        private readonly IContainer _container;

        public ServiceProviderBootstrapper(IContainer container)
        {
            _container = container;
            Initialize();
        }

        #endregion

    }

}
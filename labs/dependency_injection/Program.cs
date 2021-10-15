using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
namespace dependency_injection
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            IServiceCollection sc = new ServiceCollection();

            bool type_is_registry(Type type) => type.IsSubclassOf(typeof(Registry));

            sc.Scan(scanner => scanner
                .FromEntryAssembly()
                    .AddClasses(classes => classes.Where(type_is_registry))
                    .As<Registry>()
            );

            IServiceProvider sp = sc.BuildServiceProvider();

            var reg1 = sp.GetService<Reg1>();
            var registries = sp.GetServices<Registry>();

            ICollection<ServiceDescriptor> accumulated = new ServiceCollection();
            registries
                .SelectMany(serviceDescriptors => serviceDescriptors)
                .ToList()
                .ForEach(serviceDescriptor => accumulated.Add(serviceDescriptor));

            IServiceCollection appServices = (ServiceCollection)accumulated;

            var appSp = appServices.BuildServiceProvider();

            var iFoo = appSp.GetRequiredService<IFoo>();
            var iBar = appSp.GetRequiredService<IBar>();


            Console.WriteLine("Hello World!");
        }
    }


    public abstract class Registry : ServiceCollection { }

    public class Reg1 : Registry
    {
        public Reg1()
        {
            this.AddSingleton<IFoo, Foo>();
        }
    }

    internal class Foo : IFoo { }

    internal interface IFoo
    {
    }

    public class Reg2 : Registry
    {
        public Reg2()
        {
            this.AddSingleton<IBar, Bar>();
        }
    }

    internal class Bar : IBar { }

    internal interface IBar
    {
    }
}

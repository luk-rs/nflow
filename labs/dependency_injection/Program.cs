using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using nflow.core.Scan;

namespace dependency_injection
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            IServiceCollection sc = new ServiceCollection();

            var scan = sc.AutoScan();

            var iFoo = scan.RequiredService<IFoo>();
            var iBar = scan.RequiredService<IBar>();

            Console.WriteLine("We didn't blow up so we must be fine!");
        }
    }


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

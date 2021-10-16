using Microsoft.Extensions.DependencyInjection;
using nflow.core.Scan;

namespace nflow.core.tests.Scan.ScanData
{
    public class TestsDummyRegistry :Registry
    {
        public TestsDummyRegistry()
        {
            this.AddSingleton<IFoo, Foo>();
            this.AddTransient<IBar, Bar>();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace nflow.core.tests.ScanData.Services
{
    public class TestsDummyRegistry :Abstractions.Registry
    {
        public TestsDummyRegistry()
        {
            this.AddSingleton<IFoo, Foo>();
            this.AddTransient<IBar, Bar>();
        }
    }
}
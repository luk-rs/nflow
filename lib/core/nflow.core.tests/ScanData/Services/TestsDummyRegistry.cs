namespace nflow.core.tests.ScanData.Services
{
    using Microsoft.Extensions.DependencyInjection;
    using nflow.core;

    public class TestsDummyRegistry : Registry
    {
        public TestsDummyRegistry()
        {
            this.AddSingleton<IFoo, Foo>();
            this.AddTransient<IBar, Bar>();
        }
    }
}
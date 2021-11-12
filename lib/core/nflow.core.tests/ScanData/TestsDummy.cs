namespace nflow.core.tests.ScanData
{
    using Microsoft.Extensions.DependencyInjection;
    using nflow.core;
    using nflow.core.tests.ScanData.Services;

    public class TestsDummy : Registry
    {
        public TestsDummy()
        {
            this.AddSingleton<IFoo, Foo>();
            this.AddTransient<IBar, Bar>();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using nflow.core.Scan;
using nflow.core.tests.deps.Services;

namespace nflow.core.tests.deps
{
    public class TestsDepsDummyRegistry:Registry
    {
        public TestsDepsDummyRegistry()
        {
            this.AddSingleton<IBarDep, BarDep>();
            this.AddTransient<IFooDep, FooDep>();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using nflow.core.Abstractions;

namespace nflow.core.tests.deps.Services
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
using Microsoft.Extensions.DependencyInjection;
using nflow.core.tests.deps.Services;

namespace nflow.core.tests.deps
{
    public class TestsDepsDummy : Registry
    {
        public TestsDepsDummy()
        {
            this.AddSingleton<IBarDep, BarDep>();

            this.AddTransient<IFooDep, FooDep>();
        }
    }
}
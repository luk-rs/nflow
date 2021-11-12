using Microsoft.Extensions.DependencyInjection;
using nflow.core.Test.Services;

namespace nflow.core.Test
{

    internal class TestMicro : Registry
    {
        public TestMicro()
        {
            this.AddSingleton<Ii, Cc>();
        }
    }
}
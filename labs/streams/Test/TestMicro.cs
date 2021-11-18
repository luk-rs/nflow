using Microsoft.Extensions.DependencyInjection;
using nflow.core;
using streams.Test.Services;

namespace streams.Test
{

    internal class TestMicro : Registry
    {
        public TestMicro()
        {
            this.AddSingleton<Ii, Cc>();
        }
    }
}
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using nflow.core.tests.deps.Services;
using nflow.core.tests.ScanData.Services;
using Xunit;
using Xunit.Abstractions;

namespace nflow.core.tests.Registry
{
    public class RegistryScanTests
    {
        private readonly ITestOutputHelper _output;

        public RegistryScanTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void RegistryDeclarationsAreSuccessfullyScannedForCurrentAssembly()
        {
            // IServiceCollection collection = new ServiceCollection();
            // collection.AttachFlow(typeof(RegistryScanTests).Assembly);

            // var sut = collection.BuildServiceProvider();

            // var foo = sut.GetService<IFoo>();
            // foo.Should().NotBeNull();

            // var bar1 = sut.GetService<IBar>();
            // var bar2 = sut.GetService<IBar>();

            // bar1.Should().NotBeNull();
            // bar2.Should().NotBeNull();

            // bar1.Should().NotBeSameAs(bar2);
        }

        [Fact]
        public void RegistryDeclarationsAreSuccessfullyScannedForDependentAssembly()
        {
            // IServiceCollection collection = new ServiceCollection();
            // collection.AttachFlow(typeof(RegistryScanTests).Assembly);

            // var sut = collection.BuildServiceProvider();

            // var foo = sut.GetService<IBarDep>();
            // foo.Should().NotBeNull();

            // var bar1 = sut.GetService<IFooDep>();
            // var bar2 = sut.GetService<IFooDep>();

            // bar1.Should().NotBeNull();
            // bar2.Should().NotBeNull();

            // bar1.Should().NotBeSameAs(bar2);
        }
    }
}
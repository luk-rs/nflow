using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using nflow.core.Scan;
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
            var sut = new ServiceCollection().ScanRegistries(typeof(RegistryScanTests).Assembly);

            var foo = sut.Service<IFoo>();
            foo.Should().NotBeNull();

            var bar1 = sut.Service<IBar>();
            var bar2 = sut.Service<IBar>();

            bar1.Should().NotBeNull();
            bar2.Should().NotBeNull();

            bar1.Should().NotBeSameAs(bar2);
        }

        [Fact]
        public void RegistryDeclarationsAreSuccessfullyScannedForDependentAssembly()
        {
            var sut = new ServiceCollection().ScanRegistries(typeof(RegistryScanTests).Assembly);

            var foo = sut.Service<IBarDep>();
            foo.Should().NotBeNull();

            var bar1 = sut.Service<IFooDep>();
            var bar2 = sut.Service<IFooDep>();

            bar1.Should().NotBeNull();
            bar2.Should().NotBeNull();

            bar1.Should().NotBeSameAs(bar2);
        }
    }
}
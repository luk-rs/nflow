using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using nflow.core.Scan.Stream;
using nflow.core.tests.deps.Streams;
using nflow.core.tests.ScanData.Streams;
using Xunit;
using Xunit.Abstractions;

namespace nflow.core.tests.Stream
{
    public class StreamScanTests
    {
        private readonly ITestOutputHelper _output;
        
        public StreamScanTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void StreamsAreSuccessfullyScannedForCurrentAssembly()
        {
            var sut = new ServiceCollection().ScanStreams(typeof(StreamScanTests).Assembly);

            var barStream = sut.Private<BarStream>();
            var fooStream = sut.Public<FooStream>();

            barStream.Should().NotBeNull();
            fooStream.Should().NotBeNull();
        }

        [Fact]
        public void StreamShouldBeNullWhenRequestedWithIncorrectAccessibilityForCurrentAssembly()
        {
            var sut = new ServiceCollection().ScanStreams(typeof(StreamScanTests).Assembly);

            var barStream = sut.Public<BarStream>();
            var fooStream = sut.Private<FooStream>();

            barStream.Should().BeNull();
            fooStream.Should().BeNull();
        }
        [Fact]
        public void StreamsAreSuccessfullyScannedForDependentAssembly()
        {
            var sut = new ServiceCollection().ScanStreams(typeof(StreamScanTests).Assembly);

            var barStream = sut.Public<BarDepStream>();
            var fooStream = sut.Private<FooDepStream>();

            barStream.Should().NotBeNull();
            fooStream.Should().NotBeNull();
        }

        [Fact]
        public void StreamShouldBeNullWhenRequestedWithIncorrectAccessibilityForDependentAssembly()
        {
            var sut = new ServiceCollection().ScanStreams(typeof(StreamScanTests).Assembly);

            var barStream = sut.Private<BarDepStream>();
            var fooStream = sut.Public<FooDepStream>();

            barStream.Should().BeNull();
            fooStream.Should().BeNull();
        }

    }
}

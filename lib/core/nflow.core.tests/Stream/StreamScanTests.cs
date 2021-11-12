// using System.Linq;
// using FluentAssertions;
// using Microsoft.Extensions.DependencyInjection;

// using nflow.core.tests.deps.Scan.Streams;
// using nflow.core.tests.ScanData.Streams;
// using Xunit;
// using Xunit.Abstractions;

// namespace nflow.core.tests.Stream
// {
//     public class StreamScanTests
//     {
//         private readonly ITestOutputHelper _output;

//         public StreamScanTests(ITestOutputHelper output)
//         {
//             _output = output;
//         }

//         [Fact]
//         public void StreamsAreSuccessfullyScannedForCurrentAssembly()
//         {
//             var sut = new ServiceCollection().ScanStreams(typeof(StreamScanTests).Assembly);

//             var barStream = sut.Private<BarStream>("tests.Scan");
//             var fooStream = sut.Public<FooStream>();

//             barStream.Should().NotBeNull();
//             fooStream.Should().NotBeNull();
//         }

//         [Fact]
//         public void StreamShouldBeNullWhenRequestedWithIncorrectAccessibilityForCurrentAssembly()
//         {
//             var sut = new ServiceCollection().ScanStreams(typeof(StreamScanTests).Assembly);

//             var barStream = sut.Public<BarStream>();
//             var fooStream = sut.Private<FooStream>("tests.Scan");

//             barStream.Should().BeNull();
//             fooStream.Should().BeNull();
//         }

//         [Fact]
//         public void StreamsAreSuccessfullyScannedForDependentAssembly()
//         {
//             var sut = new ServiceCollection().ScanStreams(typeof(StreamScanTests).Assembly);

//             var barStream = sut.Public<BarDepStream>();
//             var fooStream = sut.Private<FooDepStream>("tests.deps.Scan");

//             barStream.Should().NotBeNull();
//             fooStream.Should().NotBeNull();
//         }

//         [Fact]
//         public void StreamShouldBeNullWhenRequestedWithIncorrectAccessibilityForDependentAssembly()
//         {
//             var sut = new ServiceCollection().ScanStreams(typeof(StreamScanTests).Assembly);

//             var barStream = sut.Private<BarDepStream>("tests.deps.Scan");
//             var fooStream = sut.Public<FooDepStream>();

//             barStream.Should().BeNull();
//             fooStream.Should().BeNull();
//         }

//         [Fact]
//         public void StreamShouldBeAvailableForItsMicroInCurrentAssembly()
//         {
//             var sut = new ServiceCollection().ScanStreams(typeof(StreamScanTests).Assembly);

//             var streams = sut
//                 .Micro("tests.Scan")
//                 .ToList();

//             streams.Count.Should().Be(2);
//             streams.Select(s => s.GetType())
//                 .Should()
//                 .BeEquivalentTo(new[]
//                 {
//                     typeof(BarStream),
//                     typeof(FooStream)
//                 });
//         }

//         [Fact]
//         public void StreamShouldBeAvailableForItsMicroInDependentAssembly()
//         {
//             var sut = new ServiceCollection().ScanStreams(typeof(StreamScanTests).Assembly);

//             var streams = sut
//                 .Micro("tests.deps.Scan")
//                 .ToList();

//             streams.Count.Should().Be(2);
//             streams.Select(s => s.GetType())
//                 .Should()
//                 .BeEquivalentTo(new[]
//                 {
//                     typeof(BarDepStream),
//                     typeof(FooDepStream)
//                 });
//         }
//     }
// }
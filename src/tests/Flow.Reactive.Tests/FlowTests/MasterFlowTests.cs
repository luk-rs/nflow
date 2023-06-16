namespace Flow.Reactive.Tests.FlowTests
{
    using Flow.Reactive.Tests.FlowTests.Reconnect.Commands;
    using Flow.Reactive.Tests.FlowTests.Reconnect.Streams.Public;
    using Flow.Reactive.Tests.FlowTests.TestHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class MasterFlowTests
    {
        [Test]
        public void FlowShouldEmmitUnhandledExceptionWhenAnExceptionHappensOnANano()
        {
            IFlow sut = FlowFactory.CreateFlow("Reconnect");

            Exception unhandledExceptionCaught = null;

            sut.UnhandledException.Subscribe(exception => unhandledExceptionCaught = exception);

            sut.Send(new ProduceUnhandledException(new InvalidOperationException())).Subscribe();

            unhandledExceptionCaught.Should().BeOfType<InvalidOperationException>();
        }

        [Test]
        public void IfFlowIsNotStartedAfterAnUnhhandledExceptionThanSubsequentCommandsAreIgnored()
        {
            IFlow sut = FlowFactory.CreateFlow("Reconnect");

            Exception unhandledExceptionCaught = null;

            sut.UnhandledException.Subscribe(exception => unhandledExceptionCaught = exception);

            sut.Send(new ProduceUnhandledException(new InvalidOperationException())).Subscribe();

            sut.Send(new ProduceUnhandledException(new ArgumentOutOfRangeException())).Subscribe();

            unhandledExceptionCaught.Should().BeOfType<InvalidOperationException>();
        }

        [Test]
        public void IfFlowIsStartedAfterAnUnhhandledExceptionThanSubsequentCommandsAreProcessed()
        {
            IFlow sut = FlowFactory.CreateFlow("Reconnect");

            Exception unhandledExceptionCaught = null;

            sut.UnhandledException.Subscribe(exception => unhandledExceptionCaught = exception);

            sut.Send(new ProduceUnhandledException(new InvalidOperationException())).Subscribe();

            sut.Start();

            sut.Send(new ProduceUnhandledException(new ArgumentOutOfRangeException())).Subscribe();

            unhandledExceptionCaught.Should().BeOfType<ArgumentOutOfRangeException>();
        }

        [Test]
        public void IfFlowIsRestartedAfterAnUnhhandledExceptionThanAllPersistedStreamsAreReset()
        {
            IFlow sut = FlowFactory.CreateFlow("Reconnect");

            sut
                .Query<IntCollection>()
                .Subscribe(collection => { });

            sut.Send(new ProduceUnhandledException(new InvalidOperationException())).Subscribe();

            sut.Start();

            sut
                .Query<IntCollection>()
                .Subscribe(collection => collection.Items.Count().Should().Be(1));
        }

        [Test]
        public void IfFlowIsRestartedAfterAnUnhhandledExceptionThanTablePersistedStreamsAreReset()
        {
            IFlow sut = FlowFactory.CreateFlow("Reconnect");

            sut.Send(new CommandToUpdateRecordInTable(1, "A")).Subscribe();

            sut.Send(new ProduceUnhandledException(new InvalidOperationException())).Subscribe();

            sut.Start();

            sut
              .Query<ReconnectableTable>()
              .Subscribe(collection => collection.GetData(1).Should().BeNull());
        }
    }
}

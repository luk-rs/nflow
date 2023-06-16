namespace Flow.Reactive.Tests.FlowTests.Streams.Persisted
{
    using System;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;
    using Flow.Reactive.Tests.FlowTests.TestHelpers;
    using FluentAssertions;
    using Microsoft.Reactive.Testing;
    using NUnit.Framework;

    [TestFixture]
    internal class PersistedStreamsTests
    {
        [Test]
        public void GetSnapshot_Should_ReturnTheCurrentValue()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            flow
                .GetSnapshot<PersistedStreamData1>()
                .UpdateCount
                .Should()
                .Be(0);

            flow
                .Send(new CommandA())
                .Subscribe();

            flow
               .GetSnapshot<PersistedStreamData1>()
               .UpdateCount
               .Should()
               .Be(1);
        }

        [Test]
        public void MultipleUpdatesPerformedBySameNanoSynchronouslyShouldEmitTwice()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var scheduler = new TestScheduler();

            var observer = scheduler.CreateObserver<PersistedStreamData1>();

            flow
                .Query<PersistedStreamData1>()
                .Subscribe(observer);

            flow
                .Send(new CommandToUpdateTwice())
                .Subscribe();

            observer.Messages.ShouldBe(
                (0, new PersistedStreamData1()),
                (0, new PersistedStreamData1 { UpdateCount = 1 }),
                (0, new PersistedStreamData1 { UpdateCount = 2 }));
        }
    }
}

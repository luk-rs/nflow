namespace Flow.Reactive.Tests.FlowTests.Core
{
    using Flow.Reactive.CustomExceptions;
    using Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.Commands;
    using Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.Streams.Private;
    using Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.Streams.Public;
    using Flow.Reactive.Tests.FlowTests.Reconnect.Streams.Public;
    using Flow.Reactive.Tests.FlowTests.TestHelpers;
    using FluentAssertions;
    using Microsoft.Reactive.Testing;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class FlowCoreTests
    {
        [Test]
        public void ShouldThrowPublicStreamNotFoundExceptionIfStreamIsNotPartOfMicroService()
        {
            var sut = FlowFactory.CreateFlow("MicroA");

            Action streamSubscription = () =>
            sut
                .Query<IntCollection>()
                .Subscribe();

            streamSubscription.Should().ThrowExactly<PublicStreamNotFoundException>();
        }

        [Test]
        public void ShouldThrowPublicStreamNotFoundExceptionIfStreamIsPrivate()
        {
            var sut = FlowFactory.CreateFlow("MicroA");

            Action streamSubscription = () =>
            sut
                .Query<PrivatePersistedStream1Data>()
                .Subscribe();

            streamSubscription.Should().ThrowExactly<PublicStreamNotFoundException>();
        }

        [Test]
        public void QueryWillImmediatlyReceiveTheCurrentStateOfThePersistedStream()
        {
            var sut = FlowFactory.CreateFlow("MicroA");

            var scheduler = new TestScheduler();

            var observer = scheduler.CreateObserver<PersistedStream1Data>();

            sut
                .Query<PersistedStream1Data>()
                .Subscribe(observer);

            observer.Messages.ShouldBe((0, new PersistedStream1Data()));
        }

        [Test]
        public void QueryWillReceiveTheCurrentStateOfThePersistedStreamAndSubsequentUpdates()
        {
            var sut = FlowFactory.CreateFlow("MicroA");

            var scheduler = new TestScheduler();

            var observer = scheduler.CreateObserver<PersistedStream1Data>();

            sut
                .Query<PersistedStream1Data>()
                .Subscribe(observer);

            sut
                .Send(new Command1())
                .Subscribe();

            observer.Messages.ShouldBe(
                (0, new PersistedStream1Data() { Count = 0 }),
                (0, new PersistedStream1Data() { Count = 1 }));
        }

        [Test]
        public void AMicroCanQueryStreamsForAnotherMico()
        {
            var sut = FlowFactory.CreateFlow("MicroA", "MicroB");

            var scheduler = new TestScheduler();

            var observer = scheduler.CreateObserver<EventStream1Data>();

            sut
                .Listen<EventStream1Data>()
                .Subscribe(observer);

            sut
                .Send(new Command1())
                .Subscribe();

            observer.Messages.ShouldBe((0, new EventStream1Data(1)));
        }
    }
}

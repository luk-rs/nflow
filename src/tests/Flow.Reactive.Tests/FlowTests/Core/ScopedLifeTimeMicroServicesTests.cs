namespace Flow.Reactive.Tests.FlowTests.Core
{
    using Flow.Reactive.CustomExceptions;
    using Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.Commands;
    using Flow.Reactive.Tests.FlowTests.Core.MicroServices.TransientMicro.Streams.Public;
    using Flow.Reactive.Tests.FlowTests.TestHelpers;
    using FluentAssertions;
    using Microsoft.Reactive.Testing;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ScopedLifeTimeMicroServicesTests
    {
        [Test]
        public void QueryingAPersistedStreamOfATransientMicrosReceivesTheCurrentState()
        {
            var sut = FlowFactory.CreateFlow(
                ("MicroA", Transient: false), 
                ("MicroB", Transient: false), 
                ("TransientMicro", Transient: true));

            var scheduler = new TestScheduler();

            var observer = scheduler.CreateObserver<TransientPersistedStream1Data>();

            sut
                .Query<TransientPersistedStream1Data>()
                .Subscribe(observer);

            observer.Messages.ShouldBe(
               (0, new TransientPersistedStream1Data() { Count = 0 }));
        }

        [Test]
        public void NanosOfTransientMicrosDoNotWorkIfNotStarted()
        {
            var sut = FlowFactory.CreateFlow(
                ("MicroA", Transient: false),
                ("MicroB", Transient: false),
                ("TransientMicro", Transient: true));

            var scheduler = new TestScheduler();

            var observer = scheduler.CreateObserver<TransientPersistedStream1Data>();

            sut
                .Query<TransientPersistedStream1Data>()
                .Subscribe(observer);

            sut
               .Send(new Command1())
               .Subscribe();

            observer.Messages.ShouldBe(
               (0, new TransientPersistedStream1Data() { Count = 0 }));
        }

        [Test]
        public void StartingANonExistentMicroShouldThrowAnException()
        {
            var sut = FlowFactory.CreateFlow(
               ("MicroA", Transient: false),
               ("MicroB", Transient: false));

            Action startTransient = () => sut.StartTransientMicro("TransientMicro");

            startTransient
                .Should()
                .ThrowExactly<ArgumentException>()
                .WithMessage("TransientMicro not found");
        }

        [Test]
        public void StartingANonTransientMicroShouldThrowAnException()
        {
            var sut = FlowFactory.CreateFlow(
               ("MicroA", Transient: false),
               ("MicroB", Transient: false),
               ("TransientMicro", Transient: false));

            Action startTransient = () => sut.StartTransientMicro("TransientMicro");

            startTransient
                .Should()
                .ThrowExactly<InvalidOperationException>()
                .WithMessage("TransientMicro is not transient");
        }

        [Test]
        public void NanosOfTransientMicrostWorkAftertBeingStarted()
        {
            var sut = FlowFactory.CreateFlow(
                ("MicroA", Transient: false),
                ("MicroB", Transient: false),
                ("TransientMicro", Transient: true));

            var scheduler = new TestScheduler();

            var observer = scheduler.CreateObserver<TransientPersistedStream1Data>();

            sut
                .Query<TransientPersistedStream1Data>()
                .Subscribe(observer);

            sut.StartTransientMicro("TransientMicro");

            sut
               .Send(new Command1())
               .Subscribe();

            observer.Messages.ShouldBe(
               (0, new TransientPersistedStream1Data() { Count = 0 }),
               (0, new TransientPersistedStream1Data() { Count = 1 }));
        }

        [Test]
        public void NanosOfTransientMicrostStopWorkingAftertBeingStopped()
        {
            var sut = FlowFactory.CreateFlow(
                ("MicroA", Transient: false),
                ("MicroB", Transient: false),
                ("TransientMicro", Transient: true));

            var scheduler = new TestScheduler();

            var observer = scheduler.CreateObserver<TransientPersistedStream1Data>();

            sut
                .Query<TransientPersistedStream1Data>()
                .Subscribe(observer);

            sut.StartTransientMicro("TransientMicro");

            sut
               .Send(new Command1())
               .Subscribe();

            sut.StopTransientMicro("TransientMicro");

            sut
              .Send(new Command1())
              .Subscribe();

            observer.Messages.ShouldBe(
               (0, new TransientPersistedStream1Data() { Count = 0 }),
               (0, new TransientPersistedStream1Data() { Count = 1 }),
               (0, new TransientPersistedStream1Data() { Count = 0 })); //Stop will reset PersistedSteamState
        }
    }
}

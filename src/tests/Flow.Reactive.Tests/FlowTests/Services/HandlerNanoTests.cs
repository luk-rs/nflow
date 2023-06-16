namespace Flow.Reactive.Tests.FlowTests.Services
{
    using NUnit.Framework;
    using FluentAssertions;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using System;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Reactive.Concurrency;
    using Flow.Reactive.Tests.FlowTests.TestHelpers;

    [TestFixture]
    public class HandlerNanoTests
    {
        [Test]
        public void ByDefaultTheCommandsAreProcessedOnTheSameThread()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var persistedValues = new List<int>();

            flow
                .Query<PersistedStreamData1>()
                .Subscribe(data => persistedValues.Add(data.UpdateCount));

            flow
                .Send(new CommandA())
                .Subscribe();

            persistedValues.Should().Equal(new[] { 0, 1 });
        }

        [Test]
        public void IfCommandIsSentOnDefaultSchedulerTheCommandsAreProcessedOnAnotherThread()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var persistedValues = new List<int>();

            var are = new AutoResetEvent(false);

            flow
                .Query<PersistedStreamData1>()
                .Subscribe(data =>
                {
                    persistedValues.Add(data.UpdateCount);
                    are.Set();
                });

            flow
                .Send(new CommandA())
                .SubscribeOn(Scheduler.Default)
                .Subscribe();

            are.WaitOne(millisecondsTimeout: 500);
            are.WaitOne(millisecondsTimeout: 500);

            persistedValues.Should().Equal(new[] { 0, 1 });
        }

        [Test]
        public void ByDefaultTheNotificationsAreProcessedOnTheSameThread()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var events = 0;

            flow
                .Listen<EventStreamData1>()
                .Subscribe(_ =>
                {
                    events++;
                });

            flow
                .Send(new CommandB())
                .Subscribe();

            events.Should().Be(1);
        }
    }
}

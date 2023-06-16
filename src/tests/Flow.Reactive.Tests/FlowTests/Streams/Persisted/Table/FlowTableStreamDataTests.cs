namespace Flow.Reactive.Tests.FlowTests.Streams.Persisted.Table
{
    using System;
    using System.Reactive.Linq;
    using Flow.Reactive.Extensions;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;
    using Flow.Reactive.Tests.FlowTests.TestHelpers;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class FlowTableStreamDataTests
    {
        [Test]
        public void IfTheRecordDoesNotExistNoItemsAreListened()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var queryAndUpdatesCount = 0;

            var record1 = flow.CreateFlowTable<Table, int, RecordData>(1);

            record1
                .Query()
                .Subscribe(data => queryAndUpdatesCount++);

            queryAndUpdatesCount.Should().Be(0);
        }

        [Test]
        public void IfTheRecordIsAlreadyThereAndItWasTheLastOneUpdatedThenOneItemIsListened()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var queryAndUpdatesCount = 0;

            flow
              .Send(new CommandToUpdateRecordInTable(1, "A"))
              .Subscribe();

            var record1 = flow.CreateFlowTable<Table, int, RecordData>(1);

            record1
                .Query()
                .Subscribe(data => queryAndUpdatesCount++);

            queryAndUpdatesCount.Should().Be(1);
        }

        [Test]
        public void IfTheRecordIsAlreadyThereAndItWasNotTheLastOneUpdatedThenOneItemIsListened()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var queryAndUpdatesCount = 0;

            flow
              .Send(new CommandToUpdateRecordInTable(1, "A"))
              .Subscribe();

            flow
               .Send(new CommandToUpdateRecordInTable(2, "A"))
               .Subscribe();

            var record1 = flow.CreateFlowTable<Table, int, RecordData>(1);

            record1
                .Query()
                .Subscribe(data => queryAndUpdatesCount++);

            queryAndUpdatesCount.Should().Be(1);
        }

        [Test]
        public void IfTheRecordIsAlreadyThereAndItWasUpdatedAfterwardsAndItWasTheLastOneUpdatedThenTwoItemsAreListened()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var queryAndUpdatesCount = 0;

            flow
              .Send(new CommandToUpdateRecordInTable(1, "A"))
              .Subscribe();

            var record1 = flow.CreateFlowTable<Table, int, RecordData>(1);

            record1
                .Query()
                .Subscribe(data => queryAndUpdatesCount++);


            flow
               .Send(new CommandToUpdateRecordInTable(1, "AA"))
               .Subscribe();

            queryAndUpdatesCount.Should().Be(2);
        }

        [Test]
        public void IfTheRecordIsAlreadyThereAndItWasUpdatedAfterwardsAndItWasNotTheLastOneUpdatedThenTwoItemsAreListened()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var queryAndUpdatesCount = 0;

            flow
              .Send(new CommandToUpdateRecordInTable(1, "A"))
              .Subscribe();

            var record1 = flow.CreateFlowTable<Table, int, RecordData>(1);

            record1
                .Query()
                .Subscribe(data => queryAndUpdatesCount++);


            flow
               .Send(new CommandToUpdateRecordInTable(1, "AA"))
               .Subscribe();

            flow
              .Send(new CommandToUpdateRecordInTable(2, "B"))
              .Subscribe();

            queryAndUpdatesCount.Should().Be(2);
        }

        [Test]
        public void BatchUpdatesShouldAllBeListened()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var record1 = flow.CreateFlowTable<Table, int, RecordData>(1);
            var record2 = flow.CreateFlowTable<Table, int, RecordData>(2);
            var record3 = flow.CreateFlowTable<Table, int, RecordData>(3);

            var string1 = string.Empty;
            var string2 = string.Empty;
            var string3 = string.Empty;
            record1.Query().Subscribe(data => string1 += data.Value);
            record2.Query().Subscribe(data => string2 += data.Value);
            record3.Query().Subscribe(data => string3 += data.Value);

            flow.Send(new CommandToUpdateRecordInTable(1, "1")).Subscribe();
            flow.Send(new CommandToUpdateRecordInTable(2, "2")).Subscribe();
            flow.Send(new CommandToUpdateRecordInTable(3, "3")).Subscribe();

            flow.Send(new CommandToUpdateAllRecordsInTable("B")).Subscribe();

            string1.Should().Be("1B");
            string2.Should().Be("2B");
            string3.Should().Be("3B");
        }

        [Test]
        public void IfTheRecordIsRemovedThenNoItemIsListened()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var queryAndUpdatesCount = 0;

            flow
              .Send(new CommandToUpdateRecordInTable(1, "A"))
              .Subscribe();

            flow
                .Send(new CommandToRemoveRecordInTable(1))
                .Subscribe();

            var record1 = flow.CreateFlowTable<Table, int, RecordData>(1);

            record1
                .Query()
                .Subscribe(data => queryAndUpdatesCount++);

            queryAndUpdatesCount.Should().Be(0);
        }

        [Test]
        public void UpdatingMultipleRecordsEnsuresAllItemsAreListened()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var queryAndUpdates1Count = 0;
            var queryAndUpdates2Count = 0;

            flow
                .Send(new CommandToUpdateRecordInTable(1, "A"))
                .Subscribe();

            flow
                .Send(new CommandToUpdateRecordInTable(2, "B"))
                .Subscribe();

            var record1 = flow.CreateFlowTable<Table, int, RecordData>(1);
            var record2 = flow.CreateFlowTable<Table, int, RecordData>(2);

            record1
                .Query()
                .Subscribe(_ => queryAndUpdates1Count++);

            record1
                .Query()
                .Subscribe(_ => queryAndUpdates2Count++);

            queryAndUpdates1Count.Should().Be(1);
            queryAndUpdates2Count.Should().Be(1);

            flow
                .Send(new CommandToUpdateMultipleRecords(new[] { (1, "C"), (2, "D") }))
                .Subscribe();

            queryAndUpdates1Count.Should().Be(2);
            queryAndUpdates2Count.Should().Be(2);
        }
    }
}

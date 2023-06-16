namespace Flow.Reactive.Tests.FlowTests.Streams.Persisted.Table
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Commands;
    using Flow.Reactive.Tests.FlowTests.SampleMicro.Streams.Public;
    using Flow.Reactive.Tests.FlowTests.TestHelpers;
    using FluentAssertions;
    using Microsoft.Reactive.Testing;
    using NUnit.Framework;

    [TestFixture]
    public class TablePersistedStreamDataTests
    {
        [Test]
        public void QueryingATableStreamIsJustLikeQueryingARegularPersistedStream()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var queryAndUpdatesCount = 0;

            flow
               .Query<Table>()
               .Subscribe(_ => queryAndUpdatesCount++);

            flow
                .Send(new CommandToUpdateRecordInTable(1, "A"))
                .Subscribe();

            queryAndUpdatesCount.Should().Be(2);
        }

        [Test]
        public void TableStreamCanBeFilteredToOnlySubscribeForAParticularRecordId()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            const int RecordId = 1;

            var queryAndUpdatesCount = 0;

            flow
               .Query<Table>()
               .Where(table => table.UpdatedKeys.Contains(RecordId))
               .Subscribe(_ => queryAndUpdatesCount++);

            flow
                .Send(new CommandToUpdateRecordInTable(RecordId, "A"))
                .Subscribe();

            queryAndUpdatesCount.Should().Be(1);
        }

        [Test]
        public void TableStreamCanBeFilteredToOnlySubscribeForAParticularRecordId2()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var queryAndUpdatesCount = 0;

            flow
               .Query<Table>()
               .Where(table => table.UpdatedKeys.Contains(2))
               .Subscribe(_ => queryAndUpdatesCount++);

            flow
                .Send(new CommandToUpdateRecordInTable(1, "A"))
                .Subscribe();

            queryAndUpdatesCount.Should().Be(0);
        }

        [Test]
        public void TheOrderOfInsertionIsPreserved()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var keys = new[]
            {
                "HJDS",
                "DJSSADA",
                "DSAK",
                "LJFDHFDN",
                "KODS",
                "LDJSAODSA"
            };

            keys
                .ToList()
                .ForEach(key =>
                    flow
                        .Send(new CommandToUpdateRecordInTableStringKey(key, "A"))
                        .Subscribe());

            var allKeys = flow
                .GetSnapshot<TableStringKey>()
                .GetAllKeys();

            allKeys.Should().ContainInOrder(keys);
        }


        [Test]
        public void MultipleUpdatesPerformedBySameNanoSynchronouslyShouldEmitTwice()
        {
            var flow = FlowFactory.CreateFlow("SampleMicro");

            var scheduler = new TestScheduler();

            var queryAndUpdatesCount = 0;

            flow
               .Query<Table>()
               .Where(table => table.UpdatedKeys.Contains(1))
               .Subscribe(_ => queryAndUpdatesCount++);

            flow
                .Send(new CommandToUpdateTwoDifferentRecordsInTable(1, "A", 2, "B"))
                .Subscribe();

            queryAndUpdatesCount.Should().Be(1);

            //var observer1 = scheduler.CreateObserver<RecordData>();
            //var observer2 = scheduler.CreateObserver<RecordData>();

            //flow
            //   .Query<Table>()
            //   .Select(table => table.GetData(1))
            //   .Subscribe(observer1);

            //flow
            //  .Query<Table>()
            //  .Select(table => table.GetData(2))
            //  .Subscribe(observer2);

            //observer.Messages.ShouldBe((0, new Table { }));

            //var record1 = flow
            //    .CreateFlowTable<Table, int, RecordData>(1)
            //    .Query();

            //flow
            //    .Query<PersistedStreamData1>()
            //    .Subscribe(observer);

            //flow
            //    .Send(new CommandToUpdateTwice())
            //    .Subscribe();

            //observer.Messages.ShouldBe(
            //    (0, new PersistedStreamData1()),
            //    (0, new PersistedStreamData1 { UpdateCount = 1 }),
            //    (0, new PersistedStreamData1 { UpdateCount = 2 }));
        }
    }
}

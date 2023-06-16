namespace Flow.Reactive.Tests.FlowTests.Streams.Persisted.Table
{
    using System.Linq;
    using Flow.Reactive.Streams.Persisted.Table;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    internal class TablePersistedDataTests
    {
        public record Value(string Content);

        [Test]
        public void WhenRecordIsInsertedThenUpdateKeysContainsRecordKey()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateOrInsert("1", new Value("A"));

            sut.UpdatedKeys.Should().ContainInOrder(new[] { "1" });
        }

        [Test]
        public void WhenRecordIsInsertedThenItCanBeRetrieved()
        {
            var sut = new TablePersistedData<string, Value>();

            var record = new Value("A");

            sut.UpdateOrInsert("1", record);

            sut.GetData("1").Should().Be(record);
        }

        [Test]
        public void WhenRecordIsUpdatedThenUpdateKeysContainsRecordKey()
        {
            var sut = new TablePersistedData<string, Value>();

            var originalRecord = new Value("A");

            sut.UpdateOrInsert("1", originalRecord);

            var changedRecord = new Value("B");

            sut.UpdateOrInsert("1", changedRecord);

            sut.UpdatedKeys.Should().ContainInOrder(new[] { "1" });
        }

        [Test]
        public void WhenRecordIsUpdatedThenItCanBeRetrieved()
        {
            var sut = new TablePersistedData<string, Value>();

            var originalRecord = new Value("A");

            sut.UpdateOrInsert("1", originalRecord);

            var changedRecord = new Value("B");

            sut.UpdateOrInsert("1", changedRecord);

            sut.GetData("1").Should().Be(changedRecord);
        }

        [Test]
        public void WhenMultipleRecordsWithSameDataAreAddedThenUpdateKeysContainsRecordsKeys()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateOrInsert(Enumerable.Range(1, 10).Select(id => id.ToString()).ToList(), new Value("A"));

            sut.UpdatedKeys.Should().ContainInOrder(Enumerable.Range(1, 10).Select(id => id.ToString()));
        }

        [Test]
        public void WhenMultipleRecordsWithSameDataAreUpdatedThenUpdateKeysContainsRecordsKeys()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateOrInsert(Enumerable.Range(1, 10).Select(id => id.ToString()).ToList(), new Value("A"));

            sut.UpdateOrInsert(Enumerable.Range(4, 6).Select(id => id.ToString()).ToList(), new Value("B"));

            sut.UpdatedKeys.Should().ContainInOrder(Enumerable.Range(4, 6).Select(id => id.ToString()));
        }

        [Test]
        public void WhenMultipleRecordsWithSameDataAreAddedThenTheyCanBeRetrieved()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateOrInsert(Enumerable.Range(1, 10).Select(id => id.ToString()).ToList(), new Value("A"));

            Enumerable
                .Range(1, 10)
                .ToList()
                .ForEach(id => sut.GetData(id.ToString()).Should().Be(new Value("A")));
        }

        [Test]
        public void WhenMultipleRecordsAreAddedThenUpdateKeysContainsRecordsKeys()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateMultiple(Enumerable.Range(1, 10).Select(id => (id.ToString(), new Value("A"))).ToList());

            sut.UpdatedKeys.Should().ContainInOrder(Enumerable.Range(1, 10).Select(id => id.ToString()));
        }

        [Test]
        public void WhenMultipleRecordsAreAddedThenTheyCanBeRetrieved()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateMultiple(Enumerable.Range(1, 10).Select(id => (id.ToString(), new Value("A"))).ToList());

            Enumerable
               .Range(1, 10)
               .ToList()
               .ForEach(id => sut.GetData(id.ToString()).Should().Be(new Value("A")));
        }


        [Test]
        public void UpdateAllShouldUpdateRecords()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateMultiple(Enumerable.Range(1, 10).Select(id => (id.ToString(), new Value("A"))).ToList());

            sut.UpdateAll(new Value("B"));

            Enumerable
                 .Range(1, 10)
                 .ToList()
                 .ForEach(id => sut.GetData(id.ToString()).Should().Be(new Value("B")));
        }

        [Test]
        public void TryGetDataShouldReturnFalseForUnexistingKey()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.TryGetData("1", out var data).Should().BeFalse();

            data.Should().BeNull();
        }

        [Test]
        public void TryGetDataShouldReturnTrueForExistingKey()
        {
            var sut = new TablePersistedData<string, Value>();

            var record = new Value("A");

            sut.UpdateOrInsert("1", record);

            sut.TryGetData("1", out var data).Should().BeTrue();

            data.Should().Be(record);
        }

        [Test]
        public void GetDataShouldReturnFalseForUnexistingKey()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.GetData("1").Should().BeNull();
        }

        [Test]
        public void GetDataShouldReturnDataForExistingKey()
        {
            var sut = new TablePersistedData<string, Value>();

            var record = new Value("A");

            sut.UpdateOrInsert("1", record);

            sut.GetData("1").Should().Be(record);
        }

        [Test]
        public void GetAllDataShouldReturnAllExistingData()
        {
            var sut = new TablePersistedData<string, Value>();

            var record = new Value("A");

            sut.UpdateOrInsert(Enumerable.Range(1, 10).Select(id => id.ToString()).ToList(), record);

            sut.GetAllData().Should().BeEquivalentTo(Enumerable.Range(1, 10).Select(_ => record));
        }

        [Test]
        public void RemoveShouldReturnFalseForUnexistingRecord()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.Remove("1").Should().BeFalse();
        }

        [Test]
        public void RemoveShouldReturnTrueForExistingRecord()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateOrInsert("1", new Value("A"));

            sut.Remove("1").Should().BeTrue();
        }

        [Test]
        public void RemoveShouldRemoveTheRecord()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateOrInsert("1", new Value("A"));

            sut.Remove("1");

            sut.GetData("1").Should().BeNull();
        }

        [Test]
        public void AfterRemovingARecordThenUpdateKeysContainsRecordKey()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateOrInsert("1", new Value("A"));
            sut.UpdateOrInsert("2", new Value("A"));

            sut.Remove("1");

            sut.UpdatedKeys.Should().ContainInOrder(new[] { "1" });
        }

        [Test]
        public void ClearShouldRemoveAllRecords()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateOrInsert("1", new Value("A"));
            sut.UpdateOrInsert("2", new Value("A"));

            sut.Clear();

            sut.GetAllKeys().Should().BeEmpty();
        }

        [Test]
        public void AfterClearThenUpdatedKeysShouldContainAllPreviouslyExistingKeys()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateOrInsert("1", new Value("A"));
            sut.UpdateOrInsert("2", new Value("A"));

            sut.Clear();

            sut.UpdatedKeys.Should().ContainInOrder(new[] { "1", "2" });
        }

        [Test]
        public void ContainsKeyShouldReturnFalseForUnexistingKey()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.ContainsKey("1").Should().BeFalse();
        }

        [Test]
        public void ContainsKeyShouldReturnTrueForExistingKey()
        {
            var sut = new TablePersistedData<string, Value>();

            sut.UpdateOrInsert("1", new Value("A"));

            sut.ContainsKey("1").Should().BeTrue();
        }
    }
}

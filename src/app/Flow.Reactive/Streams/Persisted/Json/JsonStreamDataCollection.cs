namespace Flow.Reactive.Streams.Persisted.Json
{
    using System;
    using System.Collections.Generic;

    public abstract class JsonStreamDataCollection<T> : JsonStreamData
    {
        private readonly DataCollection<T> _collection;

        protected JsonStreamDataCollection() => _collection = new DataCollection<T>();

        protected JsonStreamDataCollection(IEnumerable<T> initialItems) => _collection = new DataCollection<T>(initialItems);

        public IEnumerable<T> Items => _collection.Items;

        public T GetItem(Predicate<T> condition) => _collection.GetItem(condition);

        public bool TryGetItem(Predicate<T> condition, out T item) => _collection.TryGetItem(condition, out item);

        public IEnumerable<T> GetItems(Predicate<T> condition) => _collection.GetItems(condition);

        public void Add(T item) => _collection.Add(item);

        public void AddRange(IEnumerable<T> items) => _collection.AddRange(items);

        public void ReplaceAll(IEnumerable<T> items) => _collection.ReplaceAll(items);

        public void RemoveItems(Predicate<T> condition) => _collection.RemoveItems(condition);

        public void Clear() => _collection.Clear();

        public void Replace(Predicate<T> oldItem, T newItem) => _collection.Replace(oldItem, newItem);
    }
}
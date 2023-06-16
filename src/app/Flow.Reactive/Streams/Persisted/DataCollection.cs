namespace Flow.Reactive.Streams.Persisted
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DataCollection<T>
    {
        private List<T> _items;

        public DataCollection() : this(Enumerable.Empty<T>()) { }

        public DataCollection(IEnumerable<T> initialItems) => _items = new List<T>(initialItems);

        public IEnumerable<T> Items => _items;

        public T GetItem(Predicate<T> condition) => _items.Single(item => condition(item));

        public bool TryGetItem(Predicate<T> condition, out T item)
        {
            item = _items.SingleOrDefault(item => condition(item));

            return !EqualityComparer<T>.Default.Equals(item, default);
        }

        public IEnumerable<T> GetItems(Predicate<T> condition) => _items.Where(item => condition(item));

        public void Add(T item) => AddRange(new[] { item });

        public void AddRange(IEnumerable<T> items) => _items = new List<T>(_items.Concat(items));

        public void ReplaceAll(IEnumerable<T> items) => _items = new List<T>(items);

        public void RemoveItems(Predicate<T> condition)
        {
            _items.RemoveAll(condition);
            _items = new List<T>(_items);
        }

        public void Clear()
        {
            _items.Clear();
            _items = new List<T>();
        }

        public void Replace(Predicate<T> oldItem, T newItem)
        {
            var index = _items.IndexOf(_items.Single(item => oldItem(item)));
            _items = new List<T>(_items);
            _items.RemoveAt(index);
            _items.Insert(index, newItem);
        }
    }
}
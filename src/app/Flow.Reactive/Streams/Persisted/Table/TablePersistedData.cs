namespace Flow.Reactive.Streams.Persisted.Table
{
    using System.Collections.Generic;
    using System.Linq;

    public class TablePersistedData<TKey, TData> : PersistedStreamData
    {
        private readonly OrderdDictionary<TKey, TData> _data = new();

        private List<TKey> _updatedKeys = new();

        public void UpdateOrInsert(TKey key, TData data)
        {
            _data[key] = data;

            _updatedKeys = new List<TKey>(new[] { key });
        }

        public void UpdateOrInsert(IReadOnlyCollection<TKey> keys, TData data)
        {
            _updatedKeys = keys.ToList();
            _updatedKeys.ForEach(key => _data[key] = data);
        }

        public void UpdateMultiple(IReadOnlyCollection<(TKey Key, TData Data)> newValues)
        {
            _updatedKeys = newValues
                .Select(newValue => newValue.Key)
                .ToList();

            newValues
                .ToList()
                .ForEach(newValue => _data[newValue.Key] = newValue.Data);
        }

        public void UpdateAll(TData data) => UpdateOrInsert(_data.Keys.ToList(), data);

        public bool TryGetData(TKey key, out TData data)
        {
            data = _data[key];

            return data != null;
        }

        public TData GetData(TKey key) => _data[key];

        public IReadOnlyCollection<TData> GetAllData() => _data.Values.ToList();

        public bool Remove(TKey key)
        {
            if (_data[key] == null)
                return false;

            _data.Remove(key);

            _updatedKeys = new List<TKey>(new[] { key });

            return true;
        }

        public void Clear()
        {
            _updatedKeys.Clear();
            _updatedKeys.AddRange(GetAllKeys());
            _data.Clear();
        }

        public IReadOnlyCollection<TKey> UpdatedKeys => _updatedKeys;

        public bool ContainsKey(TKey key) => _data.Contains(key);

        public IReadOnlyCollection<TKey> GetAllKeys() => _data.Keys.ToList();
    }
}

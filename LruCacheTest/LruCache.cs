using System.Collections.Concurrent;

namespace LruCacheTest
{
    public class LruCache<TKey, TVal> : ILruCache<TKey, TVal> where TKey : notnull
    {
        public LruCache(int capacity)
        {
            _capacity = capacity;
            _cache = new ConcurrentDictionary<TKey, LinkedListNode<KeyValuePair<TKey, TVal>>?>();
            _linkedList = new LinkedList<KeyValuePair<TKey, TVal>>();
        }

        public bool TryGetValue(TKey key, out TVal value)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                if (node != null)
                {
                    value = node.Value.Value;
                    _linkedList.Remove(node);
                    _linkedList.AddFirst(node);
                    return true;
                }

            }
            value = default(TVal)!;
            return false;
        }

        public void AddOrUpdate(TKey key, TVal value)
        {
            var kvp = new KeyValuePair<TKey, TVal>(key, value);
            if (_cache.TryGetValue(key, out var node))
            {
                if (node == null) return;
                _linkedList.Remove(node);
                _linkedList.AddFirst(node);
                node.Value = kvp;
                return;
            }

            while (_cache.Count >= _capacity)
            {
                var last = _linkedList.Last;
                _linkedList.RemoveLast();
                if (last != null) _cache.TryRemove(last.Value.Key, out _);
            }

            var newNode = _linkedList.AddFirst(kvp);
            _cache.TryAdd(key, newNode);
        }

        #region private properties
        private readonly ConcurrentDictionary<TKey, LinkedListNode<KeyValuePair<TKey, TVal>>?> _cache;
        private readonly LinkedList<KeyValuePair<TKey, TVal>> _linkedList;
        private readonly int _capacity;
        #endregion
    }
}
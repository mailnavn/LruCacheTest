using System.Collections.Concurrent;

namespace LruCacheTest
{
    public class LruCache<TKey, TVal> where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, LinkedListNode<KeyValuePair<TKey, TVal>>?> _cache;
        private readonly LinkedList<KeyValuePair<TKey, TVal>> _linkedList;
        private readonly int _capacity;

        public LruCache(int capacity)
        {
            _capacity = capacity;
            _cache = new ConcurrentDictionary<TKey, LinkedListNode<KeyValuePair<TKey, TVal>>?>();
            _linkedList = new LinkedList<KeyValuePair<TKey, TVal>>();
        }


    }
}
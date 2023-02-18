namespace LruCacheTest
{
    public interface ILruCache<TKey, TVal>
    {
        bool TryGetValue(TKey key, out TVal value);
        void AddOrUpdate(TKey key, TVal value);
    }
}

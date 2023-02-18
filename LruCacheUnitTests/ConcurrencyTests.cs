using LruCacheTest;

namespace LruCacheUnitTests
{
    public class ConcurrencyTests
    {

        [Fact]
        public void AddOrUpdate_UpdatesKeyValuePairInCache_WhenKeyExists()
        {
            var cache = new LruCache<int, string>(3);
            cache.AddOrUpdate(1, "one");

            Parallel.For((long)0, 10, i =>
            {
                cache.AddOrUpdate(1, "new one");
                Assert.True(cache.TryGetValue(1, out string value));
                Assert.Equal("new one", value);
            });
        }

        [Fact]
        public void TryGetValue_ReturnsFalse_WhenKeyDoesNotExistInCache()
        {
            var cache = new LruCache<int, string>(3);
            cache.AddOrUpdate(1, "one");
            cache.AddOrUpdate(2, "two");
            cache.AddOrUpdate(3, "three");

            Parallel.For((long)0, 10, i =>
            {
                Assert.False(cache.TryGetValue(4, out _));
            });
        }

        [Fact]
        public void AddOrUpdate_RemovesLeastRecentlyUsedKeyValuePair_WhenCacheIsAtCapacity()
        {
            var cache = new LruCache<int, string>(2);
            cache.AddOrUpdate(1, "one");
            cache.AddOrUpdate(2, "two");

            Parallel.Invoke(
                () => cache.AddOrUpdate(3, "three"),
                () => cache.AddOrUpdate(4, "four")
            );

            Assert.False(cache.TryGetValue(1, out _));
            Assert.False(cache.TryGetValue(2, out _));
            Assert.True(cache.TryGetValue(3, out string value));
            Assert.Equal("three", value);
            Assert.True(cache.TryGetValue(4, out value));
            Assert.Equal("four", value);
        }

    }
}

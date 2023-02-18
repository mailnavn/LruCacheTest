using LruCacheTest;

namespace LruCacheUnitTests
{
    public class UnitTests
    {
        [Fact]
        public void TryGetValue_ReturnsTrue_WhenKeyExistsInCache()
        {
            var cache = new LruCache<int, string>(3);
            cache.AddOrUpdate(1, "one");
            cache.AddOrUpdate(2, "two");
            cache.AddOrUpdate(3, "three");

            var result = cache.TryGetValue(2, out var value);

            // Assert
            Assert.True(result);
            Assert.Equal("two", value);
        }

        [Fact]
        public void TryGetValue_ReturnsFalse_WhenKeyDoesNotExistInCache()
        {
            var cache = new LruCache<int, string>(3);
            cache.AddOrUpdate(1, "one");
            cache.AddOrUpdate(2, "two");
            cache.AddOrUpdate(3, "three");

            var result = cache.TryGetValue(4, out var value);

            // Assert
            Assert.False(result);
            Assert.Equal(default, value);
        }

        [Fact]
        public void AddOrUpdate_AddsKeyValuePairToCache_WhenKeyDoesNotExist()
        {
            var cache = new LruCache<int, string>(3);

            cache.AddOrUpdate(1, "one");

            // Assert
            Assert.True(cache.TryGetValue(1, out var value));
            Assert.Equal("one", value);
        }

        [Fact]
        public void AddOrUpdate_UpdatesKeyValuePairInCache_WhenKeyExists()
        {
            var cache = new LruCache<int, string>(3);
            cache.AddOrUpdate(1, "one");

            cache.AddOrUpdate(1, "new one");

            // Assert
            Assert.True(cache.TryGetValue(1, out var value));
            Assert.Equal("new one", value);
        }

        [Fact]
        public void AddOrUpdate_RemovesLeastRecentlyUsedKeyValuePair_WhenCacheIsAtCapacity()
        {
            var cache = new LruCache<int, string>(2);
            cache.AddOrUpdate(1, "one");
            cache.AddOrUpdate(2, "two");

            cache.AddOrUpdate(3, "three");

            // Assert
            Assert.False(cache.TryGetValue(1, out _));
            Assert.True(cache.TryGetValue(2, out string value));
            Assert.Equal("two", value);
            Assert.True(cache.TryGetValue(3, out value));
            Assert.Equal("three", value);
        }
    }
}
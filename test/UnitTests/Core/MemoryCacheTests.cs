using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Xunit;

namespace UnitTests.Core
{
    public class MemoryCacheTests
    {
        private readonly NoteTakingApp.Core.MemoryCache _memoryCache;
        private readonly MemoryCacheOptions _options;
        public MemoryCacheTests()
        {
            _options = new MemoryCacheOptions();
            _memoryCache = new NoteTakingApp.Core.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCache(_options));
        }

        [Fact]
        public void ShouldNotBeNull() {
            Assert.NotNull(_memoryCache);
        }

        [Fact]
        public void CanAddItem()
        {
            var item = new { Title = "Title" };
            _memoryCache.Add(item,"Item");

            Assert.Equal(item, _memoryCache.Get("Item"));
        }


        [Fact]
        public void CanRemoveItem()
        {
            var item = new { Title = "Title" };
            _memoryCache.Add(item, "Item");

            Assert.Equal(item, _memoryCache.Get("Item"));

            _memoryCache.Remove("Item");

            Assert.Null(_memoryCache.Get("Item"));
        }
    }
}

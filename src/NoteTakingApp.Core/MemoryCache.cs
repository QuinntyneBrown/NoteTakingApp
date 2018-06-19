using Microsoft.Extensions.Caching.Memory;
using System;

namespace NoteTakingApp.Core
{
    public class MemoryCache : Cache
    {
        private static IMemoryCache _cache;
        
        public MemoryCache(IMemoryCache cache)
            => _cache = cache;

        public override T Get<T>(string key) => (T)Get(key);

        public override object Get(string key)
        {
            _cache.TryGetValue(key, out dynamic value);
            return value;
        }

        public override void Add(object objectToCache, string key)
        {
            if (objectToCache == null)
            {
                _cache.Remove(key);
            }
            else
            {
                _cache.Set(key, objectToCache);
            }
        }

        public override void Add<T>(object objectToCache, string key) => Add(objectToCache, key);

        public override void Add<T>(object objectToCache, string key, double cacheDuration)
        {
            DateTime cacheEntry;

            if (objectToCache == null)
            {
                _cache.Remove(key);
            }
            else
            {
                cacheEntry = DateTime.Now;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(cacheDuration));

                _cache.Set(key, objectToCache, cacheEntryOptions);
            }
        }

        public override void Remove(string key) => _cache.Remove(key);

        public override void ClearAll()
        {
            throw new Exception();
        }

        public override bool Exists(string key) => throw new Exception();
    }
}

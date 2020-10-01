using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace caching.Controllers
{
    public static class CachingExtension
    {
        public static async Task<T> GetCacheValueAsync<T>(this IDistributedCache cache, string key) where T : class
        {
            var result = await cache.GetStringAsync(key);
            
            if (string.IsNullOrEmpty(result))return null;

            var deserializedObj = JsonConvert.DeserializeObject<T>(result);

            return deserializedObj;
        }

        public static async Task SetCacheValueAsync<T>(this IDistributedCache cache, string key, T value) where T : class
        {
            var cacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5),
                SlidingExpiration = TimeSpan.FromSeconds(30)
            };

            var result = JsonConvert.SerializeObject(value);

            await cache.SetStringAsync(key, result, cacheEntryOptions);
        }
    }
}
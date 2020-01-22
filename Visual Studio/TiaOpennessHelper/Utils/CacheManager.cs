using System;
using System.Runtime.Caching;

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {
        /// <summary>
        /// Get cache data
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCacheData(string CacheKey)
        {
            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(CacheKey))
                return cache.Get(CacheKey);
            else
                return null;
        }

        /// <summary>
        /// Set cache data
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="StoreItem"></param>
        public static void SetCacheData(string CacheKey, object StoreItem)
        {
            ObjectCache cache = MemoryCache.Default;

            // Store data in the cache    
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(999.0);
            cache.Add(CacheKey, StoreItem, cacheItemPolicy);
        }

        /// <summary>
        /// Clear cache data
        /// </summary>
        public static void ClearCacheData()
        {
            foreach (var element in MemoryCache.Default)
            {
                MemoryCache.Default.Remove(element.Key);
            }
        }

        /// <summary>
        /// Dispose specific cache data
        /// </summary>
        /// <param name="CacheKey"></param>
        public static void DisposeCacheData(string CacheKey)
        {
            MemoryCache.Default.Remove(CacheKey);
        }

        /// <summary>
        /// Get CacheKey that contains a certain string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object GetCacheDataContaining(string str)
        {
            object CacheKey = null;

            foreach (var element in MemoryCache.Default)
            {
                if (element.Key.Contains(str))
                {
                    CacheKey = element.Key;
                    break;
                }
            }

            return CacheKey;
        }
    }
}

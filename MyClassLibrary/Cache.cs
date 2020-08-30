using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Cache
    {
        ObjectCache cache;

        public Cache()
        { 
            cache = MemoryCache.Default;
        }

        /// <summary>
        /// 取得快取的動作
        /// </summary>
        /// <param name="query">進行快取取得的查詢物件</param>
        /// <returns></returns>
        public T GetCache<T>(string strCacheName)
        {
            CacheItem item = cache.GetCacheItem(strCacheName);
            return (item != null) ? (T)item.Value : default;
        }

        /// <summary>
        /// 寫入快取的動作
        /// </summary>
        /// <param name="value">寫入快取的物件</param>
        /// <returns></returns>
        public void SetCache<T>(string strCacheName, T objCacheValue, int intAbsoluteExpirationMinute = 0, int intSlidingExpirationMinute = 0)
        {
            CacheItemPolicy policy = new CacheItemPolicy();

            // 指定過期時間，如果都沒有指定，就預設7天
            if (intAbsoluteExpirationMinute > 0)
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(intAbsoluteExpirationMinute);
            else if (intSlidingExpirationMinute > 0)
                policy.SlidingExpiration = TimeSpan.FromMinutes(intSlidingExpirationMinute);
            else
                policy.SlidingExpiration = TimeSpan.FromDays(7);

            cache.Set(strCacheName, objCacheValue, policy);
        }

        /// <summary>
        /// 清除快取的動作
        /// </summary>
        /// <param name="value">清除快取的物件</param>
        /// <returns></returns>
        public void ClearCache(string strCacheName) => cache.Remove(strCacheName);
    }
}

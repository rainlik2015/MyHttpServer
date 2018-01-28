using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.IO;

namespace MyHttpServer.Cache
{
    public class RedisCache : ICache
    {
        private RedisClient redisClient = null;

        public RedisCache()
        {
            //Redis池
            PooledRedisClientManager pool = new PooledRedisClientManager("localhost");
            redisClient = pool.GetClient() as RedisClient;
        }

        public void AddOrUpdateCache(string requestUrl, Stream httpBytesStream, DateTime lastModified, int expireInSeconds)
        {
            if (httpBytesStream == null)
                return;
            var bodyBuf = httpBytesStream.ReadAllBytes();

            //由于浏览器发送过来的报文头中的If-Modified-since字段没有精确到毫秒，所有写到缓存的lastModified也不应该精确到毫秒
            var cachedData = new CacheData { Url = requestUrl, HttpBodyBytes = bodyBuf, LastModified = lastModified.OmitMillisecond() };
            if (redisClient.Exists(cachedData.Url) > 0)
            {
                redisClient.Del(cachedData.Url);
            }
            redisClient.Set<CacheData>(cachedData.Url, cachedData);
            Expire(cachedData.Url, expireInSeconds);
        }

        public Stream GetCache(string requestUrl, DateTime ifModifiedSince)
        {
            var c = redisClient.Get<CacheData>(requestUrl);
            if (c != null)
            {
                if (c.HttpBodyBytes == null)
                    return null;
                if (ifModifiedSince.CompareTo(c.LastModified) >= 0)
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Write(c.HttpBodyBytes, 0, c.HttpBodyBytes.Length);
                    return ms;
                }
            }
            return null;
        }

        public void DeleteCacheIfExists(string requestUrl)
        {
            redisClient.Del(requestUrl);
        }

        public void Expire(string requestUrl, int seconds)
        {
            redisClient.Expire(requestUrl, seconds);
        }
    }
}

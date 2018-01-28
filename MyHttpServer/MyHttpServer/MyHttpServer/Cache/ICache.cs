using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyHttpServer.Cache
{
    public interface ICache
    {
        void AddOrUpdateCache(string requestUrl, Stream httpBytesStream, DateTime lastModified, int expireInSeconds);
        Stream GetCache(string requestUrl, DateTime ifModifiedSince);
        void DeleteCacheIfExists(string requestUrl);
        void Expire(string requestUrl, int seconds);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyHttpServer.Cache
{
    /// <summary>
    /// 本地磁盘缓存
    /// 1、缓存日志，存储在Cache.txt文件中
    /// 2、缓存的HTML文档，存储在CachedHtml目录下
    /// </summary>
    /// <remarks>
    /// 缓存日志格式：
    /// Url@LastModified@ExpireInSeconds@CachedHtmlFile
    /// </remarks>
    public class LocalFileCache : ICache
    {
        private string cacheFile;
        private string htmlCachePath;
        private string baseDir;

        public LocalFileCache(string sitePath)
        {
            baseDir = sitePath;
            cacheFile = baseDir + @"\Cache.txt";
            htmlCachePath = baseDir + @"\CachedHtml";
        }

        public void AddOrUpdateCache(string requestUrl, Stream httpBytesStream, DateTime lastModified, int expireInSeconds)
        {
            DeleteCacheIfExists(requestUrl);

            //用Guid命名缓存文件
            Guid guid = Guid.NewGuid();
            var cachedPageName = string.Format("{0}.mycache", guid.ToString());//缓存文件的扩展名无所谓，因为最终都是按二进制流读写

            #region 写缓存日志文件
            using (StreamWriter sw = new StreamWriter(cacheFile, true))
            {
                sw.WriteLine(string.Format("{0}@{1}@{2}@{3}", requestUrl, lastModified.ToString(), expireInSeconds, cachedPageName));
                sw.Close();
            }
            #endregion

            #region 写缓存页面文件
            if (!Directory.Exists(htmlCachePath))
                Directory.CreateDirectory(htmlCachePath);
            using (FileStream fs = new FileStream(htmlCachePath + @"\" + cachedPageName, FileMode.Create))
            {
                httpBytesStream.ReadAllByStep(buf =>
                {
                    fs.Write(buf, 0, buf.Length);
                });
                fs.Close();
            }
            #endregion
        }

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <remarks>
        /// 如果缓存日志中没有对应项，
        /// 或者虽然日志中有对应项，但If-Modified-Since和Last-Modified校验失败，
        /// 都会返回null
        /// </remarks>
        /// <param name="requestUrl"></param>
        /// <param name="ifModifiedSince"></param>
        /// <returns></returns>
        public Stream GetCache(string requestUrl, DateTime ifModifiedSince)
        {
            Stream r = null;
            var cacheData = GetCache(requestUrl);
            if (cacheData != null)
            {
                //判断缓存是否过期
                var exp = cacheData.ExpireInSeconds;
                var lastModified = cacheData.LastModified;
                if (exp != -1 && lastModified.AddSeconds(exp).CompareTo(DateTime.Now) <= 0)
                    return null;

                //校验If-Modified-Since和Last-Modified
                if (ifModifiedSince.CompareTo(cacheData.LastModified) >= 0)
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Write(cacheData.HttpBodyBytes, 0, cacheData.HttpBodyBytes.Length);
                    r = ms;
                }
            }
            return r;
        }

        /// <summary>
        /// 单纯获取缓存日志中的对应行，不考虑缓存过期与否
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        private CacheData GetCache(string requestUrl)
        {
            CacheData r = null;
            if (!File.Exists(cacheFile))
                return r;
            using (StreamReader sr = new StreamReader(cacheFile))
            {
                do
                {
                    string line = sr.ReadLine();
                    if (line == null || line == "")
                        break;
                    var strs = line.Split('@');
                    string theUrl = strs[0];
                    DateTime lastModified = DateTime.Parse(strs[1]);
                    int exp = -1;
                    int.TryParse(strs[2], out exp);
                    string theCachedPageName = strs[3];
                    if (requestUrl == theUrl)
                    {
                        var cachedHtmlPath = string.Format(@"{0}\{1}", htmlCachePath, theCachedPageName);
                        var buf = File.ReadAllBytes(cachedHtmlPath);
                        r = new CacheData { Url = theUrl, LastModified = lastModified, CachedHtmlFile = theCachedPageName, HttpBodyBytes = buf, ExpireInSeconds = exp };
                        break;
                    }
                } while (true);
                sr.Close();
            }
            return r;
        }

        public void DeleteCacheIfExists(string requestUrl)
        {
            var existedCacheItem = GetCache(requestUrl);

            //如果缓存日志中存在该项，则需要先删除该项
            if (existedCacheItem != null)
            {
                #region 删除已存在的缓存日志记录
                DeleteCacheLog(existedCacheItem);
                #endregion

                #region 删除已存在的记录对应的缓存html文件
                DeleteCacheFile(existedCacheItem);
                #endregion
            }
        }

        private void DeleteCacheFile(CacheData existedCacheItem)
        {
            File.Delete(string.Format(@"{0}\CachedHtml\{1}", baseDir, existedCacheItem.CachedHtmlFile));
        }

        private void DeleteCacheLog(CacheData existedCacheItem)
        {
            var lines = File.ReadAllLines(cacheFile).ToList();
            lines.Remove(existedCacheItem.ToString());
            File.WriteAllLines(cacheFile, lines);
        }

        public void Expire(string requestUrl, int seconds)
        {
            var existedCacheItem = GetCache(requestUrl);
            if (existedCacheItem != null)
            {
                DeleteCacheLog(existedCacheItem);
                existedCacheItem.ExpireInSeconds = seconds;
                using (StreamWriter sw = new StreamWriter(cacheFile, true))
                {
                    sw.WriteLine(string.Format("{0}@{1}@{2}@{3}", existedCacheItem.Url, existedCacheItem.LastModified.ToString(), existedCacheItem.ExpireInSeconds, existedCacheItem.CachedHtmlFile));
                    sw.Close();
                }
            }
        }
    }
}

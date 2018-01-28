using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyHttpServer.Cache
{
    public class CacheData
    {
        public string Url { set; get; }
        public DateTime LastModified { set; get; }
        public int ExpireInSeconds { set; get; }
        public string CachedHtmlFile { set; get; }

        public byte[] HttpBodyBytes { set; get; }

        public override string ToString()
        {
            return string.Format("{0}@{1}@{2}@{3}", Url, LastModified, ExpireInSeconds, CachedHtmlFile);
        }
    }
}

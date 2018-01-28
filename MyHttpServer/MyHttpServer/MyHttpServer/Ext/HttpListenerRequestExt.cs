using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MyHttpServer.Common;
using System.IO;
using MyHttpServer.Listener;

namespace MyHttpServer
{
    public static class HttpListenerRequestExt
    {
        public static string GetLocalPath(this MyHttpRequest req,string basePath)
        {
            if (req == null)
                return null;
            var url = req.Url;
            var relativeUrl = url.AbsolutePath;
            var localPath = string.Format("{0}{1}", basePath, relativeUrl.Substring(1, relativeUrl.Length - 1).Replace('/', '\\'));
            return localPath;
        }
    }
}

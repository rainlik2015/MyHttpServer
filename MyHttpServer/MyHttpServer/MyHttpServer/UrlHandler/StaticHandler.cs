using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using MyHttpServer.Common;
using System.Web;
using MyHttpServer.Cache;
using MyHttpServer.Listener;

namespace MyHttpServer.UrlRoute
{
    /// <summary>
    /// 路由静态页面
    /// </summary>
    public class StaticHandler : BaseUrlHandler
    {
        private string localPath = null;

        public StaticHandler(string siteName, string path, ICache c)
            : base(siteName, path, c)
        {
        }
        public override Stream GetHttpBodyWithoutCache(MyHttpRequest req, MyHttpResponse resp)
        {
            resp.StatusCode = (int)HttpStatusCode.OK;
            resp.StatusDescription = "StaticOK";
            
            var fs = File.OpenRead(localPath);
            return fs;
        }

        public override bool CanHandle(MyHttpRequest req)
        {
            if (req == null)
                return false;
            var url = req.Url;
            var relativeUrl = url.AbsolutePath;
            localPath = string.Format("{0}{1}",PhysicalPath, relativeUrl.Substring(1, relativeUrl.Length - 1).Replace('/', '\\'));

            //浏览器发过来的URL字符串通常会进行编码（尤其对于URL中包含汉字的情况），所以此处要对其进行解码
            localPath = HttpUtility.UrlDecode(localPath);

            var matchedExt = ServerConfig.AcceptedFileTypes.Where(str => Path.GetExtension(localPath).ToLower() == str).FirstOrDefault();
            if (matchedExt == null)
                return false;
            var isLocalFileExist = File.Exists(localPath);
            return isLocalFileExist;
        }
    }
}
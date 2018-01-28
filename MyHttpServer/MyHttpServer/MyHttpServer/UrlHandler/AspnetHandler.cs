using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Diagnostics;
using MyHttpServer.Cache;
using MyHttpServer.Listener;

namespace MyHttpServer.UrlRoute
{
    /// <summary>
    /// 把HTTP请求转发给ASPNET，但没有成功，暂不知道如何解决?
    /// </summary>
    public class AspnetHandler : BaseUrlHandler
    {
        public AspnetHandler(string siteName, string path, ICache c = null)
            : base(siteName, path, c)
        {
        }
        private string localPath;
        public override bool CanHandle(MyHttpRequest req)
        {
            localPath = req.GetLocalPath(PhysicalPath);
            var ext = Path.GetExtension(localPath).ToLower();
            return ext == ".aspx"
                || ext == ".ashx"
                || ext == ".asmx"
                || ext == ".cshtm"
                || ext == ".cshtml";
        }
        public override Stream GetHttpBodyWithoutCache(MyHttpRequest req, MyHttpResponse resp)
        {
            //TextWriter tw = new StreamWriter(response.OutputStream);
            //var n = Path.GetFileName(req.GetLocalPath(PhysicalPath));
            //var workerReq = new SimpleWorkerRequest(n, null, tw);
            //HttpRuntime.ProcessRequest(workerReq);
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyHttpServer.Common;
using System.IO;
using System.Diagnostics;
using MyHttpServer.Cache;
using System.Net;
using MyHttpServer.Listener;

namespace MyHttpServer.UrlRoute
{
    /// <summary>
    /// 利用CGI模式解析php请求
    /// </summary>
    public class PhpHandler : BaseUrlHandler
    {
        private string localPath = null;

        public PhpHandler(string siteName, string path, ICache c = null)
            : base(siteName, path, c)
        {
        }
        public override bool CanHandle(MyHttpRequest req)
        {
            localPath = req.GetLocalPath(PhysicalPath);
            if (localPath == null)
                return false;
            var ext = Path.GetExtension(localPath);
            return ext.ToLower() == ".php";
        }

        public override Stream GetHttpBodyWithoutCache(MyHttpRequest req,MyHttpResponse resp)
        {
            Stream r = null;
            if (localPath != null)
            {
                if (!File.Exists(localPath))
                {
                    resp.StatusCode = (int)HttpStatusCode.NotFound;
                    resp.StatusDescription = "请求的PHP文档不存在！";
                    return null;
                }

                resp.StatusCode = (int)HttpStatusCode.OK;
                resp.StatusDescription = "MyPhpOK";

                //把HTTP请求转发给PHP处理
                string phpCgi = string.Format(@"{0}\PhpCgi\php.exe", Directory.GetCurrentDirectory());
                ProcessStartInfo startInfo = new ProcessStartInfo(phpCgi, localPath);
                startInfo.RedirectStandardOutput = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = false;
                var phpProcess = Process.Start(startInfo);
                var output = phpProcess.StandardOutput;
                var buf = output.BaseStream.ReadAllBytes();
                MemoryStream ms = new MemoryStream();
                ms.Write(buf, 0, buf.Length);
                r = ms;
            }
            return r;
        }
    }
}
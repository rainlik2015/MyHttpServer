using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MyHttpServer.Mvc;
using System.IO;
using MyHttpServer.Listener;

namespace MyHttpServer.ActionResults
{
    public abstract class ActionResult
    {
        /// <summary>
        /// 解析结果对象，向浏览器返回响应报文
        /// </summary>
        /// <param name="ctx"></param>
        public abstract Stream GetHttpBody(MvcContext ctx);
    }
}

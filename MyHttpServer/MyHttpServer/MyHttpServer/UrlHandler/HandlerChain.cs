using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyHttpServer.UrlRoute;
using System.Net;
using MyHttpServer.Cache;
using System.IO;
using MyHttpServer.Listener;

namespace MyHttpServer.UrlRoute
{
    /// <summary>
    /// URL路由的职责链模式实现
    /// </summary>
    public class HandlerChain : BaseUrlHandler
    {
        private List<BaseUrlHandler> routeList = new List<BaseUrlHandler>();
        private BaseUrlHandler availableHandler = null;

        public HandlerChain(string siteName, string path, ICache c)
            : base(siteName, path, c)
        {
            routeList.Add(new StaticHandler(siteName, path, c));
            routeList.Add(new PhpHandler(siteName, path, c));
            routeList.Add(new MyMvcHandler(siteName, path, c));
        }
        public override Stream GetHttpBodyWithoutCache(MyHttpRequest req, MyHttpResponse resp)
        {
            if (availableHandler != null)
                return availableHandler.GetHttpBodyWithoutCache(req,resp);
            return null;
        }
        public override bool CanHandle(MyHttpRequest req)
        {
            var url = req.Url;
            availableHandler = routeList.FirstOrDefault(r => r.CanHandle(req));
            return availableHandler != null;
        }

        public override Stream GetHttpBodyStream(MyHttpRequest req, MyHttpResponse resp)
        {
            if (availableHandler != null)
                return availableHandler.GetHttpBodyStream(req,resp);
            return null;
        }

        public override void PrepareResponseHeaders(MyHttpResponse response)
        {
            if (availableHandler != null)
                availableHandler.PrepareResponseHeaders(response);
        }
    }
}

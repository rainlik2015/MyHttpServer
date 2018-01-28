using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyHttpServer.Mvc;
using System.Net;
using System.IO;
using MyHttpServer.Listener;

namespace MyHttpServer.ActionResults
{
    public class HttpStatusCodeResult:ActionResult
    {
        private HttpStatusCode statusCode = 0;
        private string statusDescription = null;

        public HttpStatusCodeResult(HttpStatusCode status, string description = null)
        {
            statusCode = status;
            statusDescription = description;
        }
        public override Stream GetHttpBody(MvcContext ctx)
        {
            ctx.Response.StatusCode = (int)statusCode;
            if (statusDescription != null)
                ctx.Response.StatusDescription = statusDescription;
            return null;
        }
    }
}

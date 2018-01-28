using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyHttpServer.Mvc;
using System.IO;

namespace MyHttpServer.ActionResults
{
    public class ContentResult:ActionResult
    {
        private string content = null;
        public ContentResult(string c)
        {
            content = c;
        }
        public override Stream GetHttpBody(MvcContext ctx)
        {
            if (content != null)
                return content.ToStream();
            return null;
        }
    }
}

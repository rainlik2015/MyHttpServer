using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using MyHttpServer.Mvc;

namespace MyHttpServer.ActionResults
{
    public class FileResult:ActionResult
    {
        private string localPath = null;
        public FileResult(string file)
        {
            localPath = file;
        }
        public override Stream GetHttpBody(MvcContext ctx)
        {
            var fs = File.OpenRead(localPath);
            return fs;
        }
    }
}

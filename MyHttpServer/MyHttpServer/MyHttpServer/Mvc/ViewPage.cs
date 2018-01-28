using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MyHttpServer.Mvc
{
    public abstract class ViewPage
    {
        public abstract Stream GetBytes();
    }
}

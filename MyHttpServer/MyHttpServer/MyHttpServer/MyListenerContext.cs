using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHttpServer
{
    public class MyListenerContext
    {
        public MyHttpRequest Request { set; get; }
        public MyHttpResponse Response { set; get; }
    }
}

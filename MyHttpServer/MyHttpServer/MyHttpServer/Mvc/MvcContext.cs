using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;
using MyHttpServer.Listener;

namespace MyHttpServer.Mvc
{
    public class MvcContext
    {
        public Assembly UserAssembly { set; get; }
        public string ControllerName { set; get; }
        public string ActionName { set; get; }
        public MyHttpRequest Request { set; get; }
        public MyHttpResponse Response { set; get; }
        public string PhysicalPath { set; get; }
        
        public string UserNameSpace
        {
            get
            {
                return UserAssembly.GetNameSpace();
            }
        }
    }
}

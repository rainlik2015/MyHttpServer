using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MyHttpServer;
using MyHttpServer.Mvc;
using System.IO;

namespace MyWebSite.Views.Home
{
    /// <summary>
    /// 模拟cshtml编译成的临时cs源文件
    /// 实际应该是模版引擎把.cshtm文件编译成对应的临时类
    /// </summary>
    public class Index : ViewPage
    {
        public override Stream GetBytes()
        {
            string c = DateTime.Now.Millisecond.ToString();
            return c.ToStream();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MyHttpServer.Common
{
    /// <summary>
    /// 一个服务器可以容纳多个网站，每个网站对应一个端口，而每个网站又可以有若干虚拟目录
    /// 一个HttpServer实例对应一个网站
    /// </summary>
    public static class ServerConfig
    {
        private static List<string> _AcceptedFileTypes = new List<string> { ".html", ".htm", ".css", ".js", ".gif", ".ico", ".jpg", ".png", ".json" };

        /// <summary>
        /// 可处理的静态资源类型
        /// </summary>
        public static List<string> AcceptedFileTypes
        {
            get
            {
                return _AcceptedFileTypes;
            }
        }
    }
}

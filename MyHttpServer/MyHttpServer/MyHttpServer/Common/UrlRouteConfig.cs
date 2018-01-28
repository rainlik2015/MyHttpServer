using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHttpServer.Common
{
    public static class UrlRouteConfig
    {
        private static List<RouteData> urlRoutes = new List<RouteData>();

        /// <summary>
        /// 路由规则列表
        /// </summary>
        public static List<RouteData> UrlRoutes
        {
            get
            {
                return urlRoutes;
            }
            set
            {
                urlRoutes = value;
            }
        }
    }
}

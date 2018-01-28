using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;
using MyHttpServer.Listener;

namespace MyHttpServer.ModelBinder
{
    public class UrlModelBinder : BaseModelBinder
    {
        public UrlModelBinder(RouteData r, MyHttpRequest req, MethodInfo m=null)
            : base(req, m)
        {
            MatchedUrlRule = r;
        }
        public RouteData MatchedUrlRule { set; get; }
        protected override object FindParam(ParameterInfo p)
        {
            object v = null;
            var paramName = p.Name;
            var urlParamKey = MatchedUrlRule.KeyValueDic.Keys.Where(key =>
            {
                if (key.ToLower() == paramName.ToLower())//检查参数名称是否匹配
                {
                    string paramValue = MatchedUrlRule.KeyValueDic[key];
                    try
                    {
                        v = Convert.ChangeType(paramValue, p.ParameterType);//检查参数类型是否匹配
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                return false;
            }).FirstOrDefault();
            return v;//如果在URL中找到合适的参数，则继续寻找下一个参数
        }
    }
}

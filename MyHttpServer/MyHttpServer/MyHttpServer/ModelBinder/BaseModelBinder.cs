using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;
using MyHttpServer.Listener;

namespace MyHttpServer.ModelBinder
{
    /// <summary>
    /// 在Req中寻找Method的参数
    /// </summary>
    public abstract class BaseModelBinder
    {
        protected MyHttpRequest Req { set; get; }
        public MethodInfo Method { set; get; }

        public List<object> Parse()
        {
            List<object> r = new List<object>();
            if (Method == null)
                return null;
            var methodParams = Method.GetParameters();
            foreach (var p in methodParams)
            {
                var v = FindParam(p);
                if (v == null)
                    return null;
                r.Add(v);
            }
            return r;
        }
        protected abstract object FindParam(ParameterInfo p);

        public BaseModelBinder(MyHttpRequest req, MethodInfo m=null)
        {
            Req = req;
            Method = m;
        }
    }
}

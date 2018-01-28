using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;
using System.IO;
using MyHttpServer.Listener;

namespace MyHttpServer.ModelBinder
{
    public class BodyModelBinder:BaseModelBinder
    {
        private StringDictionary bodyData = new StringDictionary();
        public BodyModelBinder(MyHttpRequest req, MethodInfo m=null)
            : base(req, m)
        {
            GetBodyData(Req.InputStream);
        }
        private void GetBodyData(Stream input)
        {
            if (input == null)
                return;
            if (bodyData.Keys.Count > 0)//正文数据只读一次
                return;
            byte[] buf = new byte[100];//缓冲区大小如何确定?
            var cnt = input.Read(buf, 0, buf.Length);
            var body = System.Text.Encoding.UTF8.GetString(buf);
            bodyData = body.GetData();
        }
        protected override object FindParam(ParameterInfo p)
        {
            if (Req.HasEntityBody)
            {
                var byName = bodyData.Keys.Where(k => k.ToLower() == p.Name.ToLower()).FirstOrDefault();
                if (byName != null)//检查参数名称
                {
                    try
                    {
                        var v = Convert.ChangeType(bodyData[byName], p.ParameterType);//检查参数类型
                        return v;
                    }
                    catch
                    { }
                }
                var model = Activator.CreateInstance(p.ParameterType);
                var paramProps = p.ParameterType.GetProperties();
                if (paramProps.Length <= 0)
                {
                    return null;
                }
                foreach (var paramProp in paramProps)
                {
                    var byNameKey = bodyData.Keys.Where(k => k.ToLower() == paramProp.Name.ToLower()).FirstOrDefault();
                    if (byNameKey != null)
                    {
                        try
                        {
                            var v = Convert.ChangeType(bodyData[byNameKey], paramProp.PropertyType);
                            paramProp.SetValue(model, v, null);
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
                return model;
            }
            return null;
        }
    }
}
